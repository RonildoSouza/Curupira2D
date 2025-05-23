﻿using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions.TiledMap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TiledLib;
using TiledLib.Layer;
using TiledLib.Objects;

namespace Curupira2D.ECS.Systems.Drawables
{
    [RequiredComponent(typeof(TiledMapSystem), typeof(TiledMapComponent))]
    public sealed class TiledMapSystem : System, ILoadable, IRenderable
    {
        public void LoadContent()
        {
            var entities = Scene.GetEntities(_ => MatchActiveEntitiesAndComponents(_));

            for (int i = 0; i < entities.Count; i++)
            {
                var tiledMapEntity = entities[i];
                var tiledMapComponent = tiledMapEntity.GetComponent<TiledMapComponent>();

                foreach (var objectLayer in tiledMapComponent.Map.GetAll<ObjectLayer>(_ => _.Visible))
                {
                    CreateCollisionEntities(objectLayer, tiledMapComponent.Map);

                    // Gets entity with the same name as the point object (spawn) and sets the position
                    foreach (var pointObject in objectLayer.GetAll<PointObject>())
                    {
                        var entity = Scene
                            .GetEntities(_ => _.UniqueId == pointObject.Name || _.UniqueId == pointObject.Properties.GetValue(TiledMapSystemConstants.Properties.EntityUniqueId))
                            .FirstOrDefault();

                        if (entity != null && entity.Position == default)
                            entity.SetPosition(pointObject.ToVector2(tiledMapComponent.Map, Scene));
                    }
                }
            }
        }

        public void Draw(ref IReadOnlyList<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var tiledMapEntity = entities[i];
                var tiledMapComponent = tiledMapEntity.GetComponent<TiledMapComponent>();

                if (tiledMapComponent == null)
                    continue;

                var layers = tiledMapComponent.Map.Layers
                    .OfType<TileLayer>()
                    .OrderBy(_ =>
                    {
                        var propertyValueOrder = _.Properties.GetValue(TiledMapSystemConstants.Properties.Order);
                        return string.IsNullOrEmpty(propertyValueOrder) ? _.Id : int.Parse(propertyValueOrder);
                    });

                foreach (var layer in layers.Where(_ => _.Visible))
                {
                    var propertyValueOrder = layer.Properties.GetValue(TiledMapSystemConstants.Properties.Order);
                    var valueOrder = string.IsNullOrEmpty(propertyValueOrder) ? layer.Id : int.Parse(propertyValueOrder);

                    if (!string.IsNullOrEmpty(propertyValueOrder) && valueOrder > layers.Count())
                        throw new InvalidOperationException($"Tile layer \"{layer.Name}\" has an invalid {TiledMapSystemConstants.Properties.Order} value. " +
                            $"The value must be less than or equal to the number of tile layers.");

                    for (int y = 0; y < layer.Height; y++)
                    {
                        for (int x = 0; x < layer.Width; x++)
                        {
                            if (!layer.HasTile(x, y))
                                continue;

                            var gid = layer.GetGlobalTileId(x, y);
                            var id = Utils.GetId(gid);
                            var tileset = tiledMapComponent.Map.Tilesets.Single(_ => id >= _.FirstGid && _.FirstGid + _.TileCount > id);
                            var tile = tileset[gid];

                            var (tileSpriteEffect, tileRotation) = GetTileOrientation(tile);
                            var (tilePosX, tilePosY) = GetTilePositionsToScreen(x, y, tile, tiledMapComponent.Map);

                            Scene.SpriteBatch.Draw(
                                tiledMapComponent.Texture,
                                new Rectangle(tilePosX, tilePosY, tile.Width, tile.Height),
                                new Rectangle(tile.Left, tile.Top, tile.Width, tile.Height),
                                Color.White * (float)layer.Opacity,
                                MathHelper.ToRadians(tileRotation),
                                tiledMapComponent.Origin,
                                tileSpriteEffect | tiledMapComponent.SpriteEffect,
                                (layers.Count() - valueOrder) / 1000f);
                        }
                    }
                }
            }
        }

        void CreateCollisionEntities(ObjectLayer objectLayer, Map map)
        {
            var baseObjects = objectLayer.Objects
                .Where(_ => _.GetType() != typeof(PointObject))
                // Visible property not set in ReadObject method of TiledLib
                .Where(_ => _.Properties.GetValue(TiledMapSystemConstants.Properties.Visible) is null
                    || bool.Parse(_.Properties.GetValue(TiledMapSystemConstants.Properties.Visible)))
                .OrderBy(_ =>
                {
                    var propertyValueOrder = _.Properties.GetValue(TiledMapSystemConstants.Properties.Order);
                    return string.IsNullOrEmpty(propertyValueOrder) ? _.Id : int.Parse(propertyValueOrder);
                });

            foreach (var baseObject in baseObjects)
            {
                var entityUniqueId = baseObject.Properties.GetValue(TiledMapSystemConstants.Properties.EntityUniqueId);
                var entityGroup = baseObject.Properties.GetValue(TiledMapSystemConstants.Properties.EntityGroup);

                // Creates ellipse type collision entity
                if (baseObject is EllipseObject ellipseObject)
                {
                    var posX = (float)ellipseObject.X + (float)ellipseObject.Width * 0.5f;
                    var posY = Scene.InvertPositionY((float)ellipseObject.Y + (float)ellipseObject.Height * 0.5f);
                    var ellipseBodyComponent = new BodyComponent((float)ellipseObject.Width, (float)ellipseObject.Height, EntityType.Static, EntityShape.Ellipse);

                    SetPhysicsProperties(ellipseObject, objectLayer, ref ellipseBodyComponent);

                    Scene.CreateEntity(entityUniqueId ?? $"{nameof(EllipseObject)}_{ellipseObject.Id}", posX, posY, entityGroup)
                        .AddComponent(ellipseBodyComponent);
                }

                // Creates rectangle type collision entity
                if (baseObject is RectangleObject rectangleObject)
                {
                    var posX = (float)rectangleObject.X + (float)rectangleObject.Width * 0.5f;
                    var posY = Scene.InvertPositionY((float)rectangleObject.Y + (float)rectangleObject.Height * 0.5f);
                    var rectangleBodyComponent = new BodyComponent((float)rectangleObject.Width, (float)rectangleObject.Height, EntityType.Static, EntityShape.Rectangle);

                    SetPhysicsProperties(rectangleObject, objectLayer, ref rectangleBodyComponent);

                    Scene.CreateEntity(entityUniqueId ?? $"{nameof(RectangleObject)}_{rectangleObject.Id}", posX, posY, entityGroup)
                        .AddComponent(rectangleBodyComponent);
                }

                // Creates polygon type collision entity
                if (baseObject is PolygonObject polygonObject)
                {
                    var (position, vertices) = GetPositionAndVerticesOfPolyObjects(polygonObject, map);
                    var polygonBodyComponent = new BodyComponent((float)polygonObject.Width, (float)polygonObject.Height, EntityType.Static, EntityShape.Polygon)
                    {
                        Vertices = vertices
                    };

                    SetPhysicsProperties(polygonObject, objectLayer, ref polygonBodyComponent);

                    Scene.CreateEntity(entityUniqueId ?? $"{nameof(PolygonObject)}_{polygonObject.Id}", position, entityGroup)
                        .AddComponent(polygonBodyComponent);
                }

                // Creates poly line type collision entity
                if (baseObject is PolyLineObject polyLineObject)
                {
                    var (position, vertices) = GetPositionAndVerticesOfPolyObjects(polyLineObject, map);
                    var polyLineBodyComponent = new BodyComponent(vertices, EntityType.Static, EntityShape.PolyLine);

                    SetPhysicsProperties(polyLineObject, objectLayer, ref polyLineBodyComponent);

                    Scene.CreateEntity(entityUniqueId ?? $"{nameof(PolyLineObject)}_{polyLineObject.Id}", position, entityGroup)
                        .AddComponent(polyLineBodyComponent);
                }
            }
        }

        static (SpriteEffects TileSpriteEffect, float TileRotation) GetTileOrientation(Tile tile)
        {
            var tileSpriteEffect = SpriteEffects.None;
            var tileRotation = 0f;

            switch (tile.Orientation)
            {
                case TileOrientation.FlippedH:
                    tileSpriteEffect = SpriteEffects.FlipHorizontally;
                    break;
                case TileOrientation.FlippedV:
                    tileSpriteEffect = SpriteEffects.FlipHorizontally;
                    tileRotation = 180f;
                    break;
                case TileOrientation.FlippedAD:
                    tileSpriteEffect = SpriteEffects.FlipHorizontally;
                    tileRotation = 90f;
                    break;
                case TileOrientation.Rotate90CW:
                    tileRotation = -90f;
                    break;
                case TileOrientation.Rotate180CCW:
                    tileSpriteEffect = SpriteEffects.FlipHorizontally;
                    tileRotation = 270f;
                    break;
                case TileOrientation.Rotate270CCW:
                    tileSpriteEffect = SpriteEffects.FlipVertically;
                    tileRotation = -270f;
                    break;
            }

            return (tileSpriteEffect, tileRotation);
        }

        static void SetPhysicsProperties(BaseObject baseObject, ObjectLayer objectLayer, ref BodyComponent bodyComponent)
        {
            var restitution = baseObject.Properties.GetValue(TiledMapSystemConstants.Properties.Physics.Restitution)
                ?? objectLayer.Properties.GetValue(TiledMapSystemConstants.Properties.Physics.Restitution);

            var friction = baseObject.Properties.GetValue(TiledMapSystemConstants.Properties.Physics.Friction)
                ?? objectLayer.Properties.GetValue(TiledMapSystemConstants.Properties.Physics.Friction);

            bodyComponent.Restitution = float.TryParse(restitution, out float restitutionValue) ? restitutionValue : bodyComponent.Restitution;
            bodyComponent.Friction = float.TryParse(friction, out float frictionValue) ? frictionValue : bodyComponent.Friction;
        }

        (int TilePosX, int TilePosY) GetTilePositionsToScreen(int x, int y, Tile tile, Map map)
        {
            int tilePosX;
            int tilePosY;

            if (map.Orientation == Orientation.isometric)
            {
                var width = tile.Width != map.CellWidth ? map.CellWidth : tile.Width;
                var height = tile.Height != map.CellHeight ? map.CellHeight : tile.Height;

                tilePosX = (x - y) * (int)(width * 0.5f);
                tilePosY = (int)Scene.InvertPositionY((x + y) * (int)(height * 0.5f));

                return (tilePosX, tilePosY);
            }

            tilePosX = x * tile.Width + (int)(tile.Width * 0.5f);
            tilePosY = (int)Scene.InvertPositionY(y * tile.Height + (int)(tile.Height * 0.5f));

            return (tilePosX, tilePosY);
        }

        (Vector2 Position, IEnumerable<Vector2> Vertices) GetPositionAndVerticesOfPolyObjects(BaseObject baseObject, Map map)
        {
            Vector2 position;
            IEnumerable<Vector2> vertices = [];
            IEnumerable<Position> positions = [];

            if (baseObject is PolygonObject polygonObject)
                positions = polygonObject.Polygon;

            if (baseObject is PolyLineObject polyLineObject)
                positions = polyLineObject.Polyline;

            if (map.Orientation == Orientation.isometric)
            {
                var positionOffset = baseObject.GetIsometricOffsetPositionY(map);

                position = new Vector2((float)baseObject.X - (float)baseObject.Y, Scene.InvertPositionY((float)baseObject.X + (float)baseObject.Y) + positionOffset);
                vertices = positions.Select(_ => CartesianToIsometricOfPolyObjects(_.X, _.Y));

                return (position, vertices);
            }

            position = new Vector2((float)baseObject.X, Scene.InvertPositionY((float)baseObject.Y));
            vertices = positions.Select(_ => new Vector2((float)_.X, (float)baseObject.Height - (float)_.Y));

            return (position, vertices);

            static Vector2 CartesianToIsometricOfPolyObjects(double x, double y)
                => new() { X = (float)(x - y), Y = (float)(-(x + y) * 0.5f) };
        }
    }
}
