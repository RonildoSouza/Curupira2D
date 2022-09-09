﻿using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Diagnostics;
using tainicom.Aether.Physics2D.Dynamics;

namespace Curupira2D.ECS.Systems.Physics
{
    [RequiredComponent(typeof(PhysicsSystem), typeof(BodyComponent))]
    public sealed class PhysicsSystem : System, ILoadable, IUpdatable
    {
        World _world;
        DebugView _debugView;

        public void LoadContent()
        {
            _world = new World();

            // enable multithreading
            _world.ContactManager.VelocityConstraintsMultithreadThreshold = 256;
            _world.ContactManager.PositionConstraintsMultithreadThreshold = 256;
            _world.ContactManager.CollideMultithreadThreshold = 256;

            var entities = Scene.GetEntities(_ => MatchActiveEntitiesAndComponents(_));

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var bodyComponent = entity.GetComponent<BodyComponent>();
                var drawableComponent = entity.GetDrawableComponent();

                Fixture fixture = null;

                switch (bodyComponent.EntityShape)
                {
                    case EntityShape.Circle:
                        fixture = bodyComponent.CreateCircle(bodyComponent.Radius, bodyComponent.Density, -(drawableComponent?.Origin - drawableComponent?.Half) ?? Vector2.Zero);
                        break;
                    case EntityShape.Ellipse:
                        fixture = bodyComponent.CreateEllipse(bodyComponent.Size.X * 0.5f, bodyComponent.Size.Y * 0.5f, 8, bodyComponent.Density);
                        break;
                    case EntityShape.Rectangle:
                        fixture = bodyComponent.CreateRectangle(bodyComponent.Size.X, bodyComponent.Size.Y, bodyComponent.Density, -(drawableComponent?.Origin - drawableComponent?.Half) ?? Vector2.Zero);
                        break;
                    case EntityShape.Polygon:
                        var vertices = new Vertices(bodyComponent.Vertices);
                        fixture = bodyComponent.CreatePolygon(vertices, bodyComponent.Density);
                        break;
                }

                _world.Add(bodyComponent);

                bodyComponent.Tag = entity.UniqueId;
                bodyComponent.Position = entity.Position;
                bodyComponent.Rotation = entity.Rotation;

                if (fixture != null)
                {
                    fixture.Restitution = bodyComponent.Restitution;
                    fixture.Friction = bodyComponent.Friction;
                }
            };

            if (Scene.GameCore.DebugActive && entities.Any())
            {
                _debugView = new DebugView(_world);
                _debugView.AppendFlags(DebugViewFlags.Shape);
                _debugView.AppendFlags(DebugViewFlags.Joint);
                _debugView.AppendFlags(DebugViewFlags.PerformanceGraph);
                _debugView.AppendFlags(DebugViewFlags.DebugPanel);
                _debugView.AppendFlags(DebugViewFlags.CenterOfMass);
                _debugView.DefaultShapeColor = Color.Orange;
                _debugView.SleepingShapeColor = Color.DodgerBlue;
                _debugView.TextColor = Color.Black;
                _debugView.StaticShapeColor = Color.Red;

                _debugView.LoadContent(Scene.GameCore.GraphicsDevice, Scene.GameCore.Content);
            }
        }

        public void Update()
        {
            var entities = Scene.GetEntities(_ => MatchActiveEntitiesAndComponents(_));

            if (Scene.Gravity != default && Scene.Gravity != _world.Gravity)
                _world.Gravity = Scene.Gravity;

            if (entities.Any() && entities.Count != _world.BodyList.Count)
            {
                Task.Factory.StartNew(() =>
                {
                    var exceptUniqueIds = entities.Select(_ => _.UniqueId).Except(_world.BodyList.Select(_ => _.Tag.ToString()));
                    var bodiesToDestroy = _world.BodyList.Where(_ => exceptUniqueIds.Contains(_.Tag.ToString()));

                    foreach (var body in bodiesToDestroy)
                        _world.Remove(body);
                });
            }

            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities.ElementAt(i);
                var bodyComponent = entity.GetComponent<BodyComponent>();

                bodyComponent.Enabled = entity.Active;

                entity.SetPosition(bodyComponent.Position);
                entity.SetRotation(MathHelper.ToDegrees(bodyComponent.Rotation));
            }

            _world.Step(Scene.DeltaTime);

            if (Scene.GameCore.DebugActive && entities.Any())
                _debugView.UpdatePerformanceGraph(_world.UpdateTime);
        }

        internal void DrawDebugData()
        {
            if (Scene.GameCore.DebugActive && Scene.ExistsEntities(_ => MatchActiveEntitiesAndComponents(_)))
            {
                if (Scene.GameCore.DebugWithUICamera2D)
                    _debugView.RenderDebugData(Scene.UICamera2D.Projection, Scene.UICamera2D.View);
                else
                    _debugView.RenderDebugData(Scene.Camera2D.Projection, Scene.Camera2D.View);
            }
        }
    }
}
