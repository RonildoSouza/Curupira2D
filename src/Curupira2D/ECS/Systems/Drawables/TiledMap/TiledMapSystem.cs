using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions.TiledMap;
using Microsoft.Xna.Framework;
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
        private static TiledMapComponent _tiledMapComponent;
        private static List<TileLayer> _tileLayers;
        private static readonly Dictionary<int, ITileset> _tilesetCached = [];
        private static readonly Dictionary<int, List<TileInfo>> _tilesToDrawCached = [];

        public void LoadContent()
        {
            _tiledMapComponent = Scene.GetEntities(MatchActiveEntitiesAndComponents).Single().GetComponent<TiledMapComponent>();
            _tileLayers = [.. _tiledMapComponent.Map.Layers
                .OfType<TileLayer>()
                .OrderBy(_ =>
                {
                    var propertyValueOrder = _.Properties.GetValue(TiledMapSystemConstants.Properties.Order);
                    return string.IsNullOrEmpty(propertyValueOrder) ? _.Id : int.Parse(propertyValueOrder);
                })];

            GetTilesToDraw(_tiledMapComponent, _tileLayers);

            foreach (var objectLayer in _tiledMapComponent.Map.GetAll<ObjectLayer>(_ => _.Visible))
            {
                CreateCollisionEntities(objectLayer, _tiledMapComponent.Map);

                // Gets entity with the same name as the point object (spawn) and sets the position
                foreach (var pointObject in objectLayer.GetAll<PointObject>())
                {
                    var entity = Scene
                        .GetEntities(_ => _.UniqueId == pointObject.Name || _.UniqueId == pointObject.Properties.GetValue(TiledMapSystemConstants.Properties.EntityUniqueId))
                        .FirstOrDefault();

                    if (entity != null && entity.Position == default)
                        entity.SetPosition(pointObject.ToVector2(_tiledMapComponent.Map));
                }
            }
        }

        public void Draw(ref IReadOnlyCollection<Entity> entities)
        {
            foreach (var layer in _tileLayers)
            {
                if (!layer.Visible)
                    continue;

                var propertyValueOrder = layer.Properties.GetValue(TiledMapSystemConstants.Properties.Order);
                var valueOrder = string.IsNullOrEmpty(propertyValueOrder) ? layer.Id : int.Parse(propertyValueOrder);

                if (!string.IsNullOrEmpty(propertyValueOrder) && valueOrder > _tileLayers.Count)
                    throw new InvalidOperationException($"Tile layers \"{layer.Name}\" has an invalid {TiledMapSystemConstants.Properties.Order} value. " +
                        $"The value must be less than or equal to the number of tile layers.");

                foreach (var tileInfo in _tilesToDrawCached.Where(_ => _.Key == layer.Id).SelectMany(_ => _.Value))
                {
                    var tilePosition = Scene.Camera2D.WorldToScreen(tileInfo.Position.ToVector2());

                    if (!Scene.UICamera2D.IsInView(
                        tilePosition.X - tileInfo.Tile.Width, tilePosition.Y - tileInfo.Tile.Height, Scene.ScreenWidth, Scene.ScreenHeight))
                        continue;

                    var destinationRectangle = new Rectangle(tileInfo.Position.X, tileInfo.Position.Y, tileInfo.Tile.Width, tileInfo.Tile.Height);
                    var sourceRectangle = new Rectangle(tileInfo.Tile.Left, tileInfo.Tile.Top, tileInfo.Tile.Width, tileInfo.Tile.Height);

                    Scene.SpriteBatch.Draw(
                        _tiledMapComponent.Texture,
                        destinationRectangle,
                        sourceRectangle,
                        Color.White * (float)layer.Opacity,
                        MathHelper.ToRadians(tileInfo.Rotation),
                        _tiledMapComponent.Origin,
                        tileInfo.SpriteEffect | _tiledMapComponent.SpriteEffect,
                        (_tileLayers.Count - valueOrder) / 1000f);
                }
            }
        }

        static ITileset GetTilesetCached(int tileLayerId, TiledMapComponent tiledMapComponent)
        {
            if (!_tilesetCached.TryGetValue(tileLayerId, out var tileset))
            {
                tileset = tiledMapComponent.Map.Tilesets.Length == 1
                    ? tiledMapComponent.Map.Tilesets[0]
                    : tiledMapComponent.Map.Tilesets.First(ts => tileLayerId >= ts.FirstGid && tileLayerId < ts.FirstGid + ts.TileCount);

                _tilesetCached[tileLayerId] = tileset;
            }

            return tileset;
        }

        static void GetTilesToDraw(TiledMapComponent tiledMapComponent, List<TileLayer> tileLayers)
        {
            foreach (var tileLayer in tileLayers)
            {
                if (!tileLayer.Visible)
                    continue;

                List<TileInfo> tilesToDraw = [];

                for (int y = 0; y < tileLayer.Height; y++)
                {
                    for (int x = 0; x < tileLayer.Width; x++)
                    {
                        if (!tileLayer.HasTile(x, y))
                            continue;

                        var gid = tileLayer.GetGlobalTileId(x, y);
                        var id = Utils.GetId(gid);
                        var tileset = GetTilesetCached(id, tiledMapComponent);
                        var tile = tileset[gid];
                        var tilePosition = GetTilePositionsToScreen(x, y, tile, tiledMapComponent.Map);

                        tilesToDraw.Add(new TileInfo(tile, tilePosition));
                    }
                }

                _tilesToDrawCached.Add(tileLayer.Id, tilesToDraw);
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
                    var posY = (float)ellipseObject.Y + (float)ellipseObject.Height * 0.5f;
                    var ellipseBodyComponent = new BodyComponent((float)ellipseObject.Width, (float)ellipseObject.Height, EntityType.Static, EntityShape.Ellipse);

                    SetPhysicsProperties(ellipseObject, objectLayer, ref ellipseBodyComponent);

                    Scene.CreateEntity(entityUniqueId ?? $"{nameof(EllipseObject)}_{ellipseObject.Id}", posX, posY, entityGroup)
                        .AddComponent(ellipseBodyComponent);
                }

                // Creates rectangle type collision entity
                if (baseObject is RectangleObject rectangleObject)
                {
                    var posX = (float)rectangleObject.X + (float)rectangleObject.Width * 0.5f;
                    var posY = (float)rectangleObject.Y + (float)rectangleObject.Height * 0.5f;
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

        static void SetPhysicsProperties(BaseObject baseObject, ObjectLayer objectLayer, ref BodyComponent bodyComponent)
        {
            var restitution = baseObject.Properties.GetValue(TiledMapSystemConstants.Properties.Physics.Restitution)
                ?? objectLayer.Properties.GetValue(TiledMapSystemConstants.Properties.Physics.Restitution);

            var friction = baseObject.Properties.GetValue(TiledMapSystemConstants.Properties.Physics.Friction)
                ?? objectLayer.Properties.GetValue(TiledMapSystemConstants.Properties.Physics.Friction);

            bodyComponent.Restitution = float.TryParse(restitution, out float restitutionValue) ? restitutionValue : bodyComponent.Restitution;
            bodyComponent.Friction = float.TryParse(friction, out float frictionValue) ? frictionValue : bodyComponent.Friction;
        }

        static Point GetTilePositionsToScreen(int x, int y, Tile tile, Map map)
        {
            int tilePosX;
            int tilePosY;

            if (map.Orientation == Orientation.isometric)
            {
                var width = tile.Width != map.CellWidth ? map.CellWidth : tile.Width;
                var height = tile.Height != map.CellHeight ? map.CellHeight : tile.Height;

                tilePosX = (x - y) * (int)(width * 0.5f);
                tilePosY = (x + y) * (int)(height * 0.5f);

                return new Point(tilePosX, tilePosY);
            }

            tilePosX = x * tile.Width + (int)(tile.Width * 0.5f);
            tilePosY = y * tile.Height + (int)(tile.Height * 0.5f);

            return new Point(tilePosX, tilePosY);
        }

        static (Vector2 Position, IEnumerable<Vector2> Vertices) GetPositionAndVerticesOfPolyObjects(BaseObject baseObject, Map map)
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
                var isometricOffsetPositionY = baseObject.GetIsometricOffsetPositionY(map);

                position = new Vector2((float)baseObject.X - (float)baseObject.Y, (float)baseObject.X + (float)baseObject.Y - isometricOffsetPositionY);
                vertices = positions.Select(_ => CartesianToIsometricOfPolyObjects(_.X, _.Y));

                return (position, vertices);
            }

            position = new Vector2((float)baseObject.X, (float)baseObject.Y);
            vertices = positions.Select(_ => new Vector2((float)_.X, (float)_.Y));

            return (position, vertices);

            static Vector2 CartesianToIsometricOfPolyObjects(double x, double y)
                => new() { X = (float)(x - y), Y = (float)((x + y) * 0.5f) };
        }
    }
}
