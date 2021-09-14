using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems.Attributes;
using Microsoft.Xna.Framework;
using System.Linq;
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

                switch (bodyComponent.EntityShape)
                {
                    case EntityShape.Circle:
                        bodyComponent.CreateCircle(bodyComponent.Radius, bodyComponent.Density, Vector2.Zero);
                        break;
                    case EntityShape.Ellipse:
                        bodyComponent.CreateEllipse(bodyComponent.Size.X * 0.5f, bodyComponent.Size.Y * 0.5f, 8, bodyComponent.Density);
                        break;
                    case EntityShape.Rectangle:
                        bodyComponent.CreateRectangle(bodyComponent.Size.X, bodyComponent.Size.Y, bodyComponent.Density, Vector2.Zero);
                        break;
                    case EntityShape.Polygon:
                        var vertices = new Vertices(bodyComponent.Vertices);
                        bodyComponent.CreatePolygon(vertices, bodyComponent.Density);
                        break;
                }

                _world.Add(bodyComponent);

                bodyComponent.Tag = entity.UniqueId;
                bodyComponent.Position = entity.Transform.Position;
                bodyComponent.Rotation = entity.Transform.Rotation;
                bodyComponent.Restitution = bodyComponent.Restitution;
                bodyComponent.Friction = bodyComponent.Friction;
            };

            if (Scene.GameCore.DebugActive && entities.Any())
            {
                _debugView = new DebugView(_world);
                _debugView.AppendFlags(DebugViewFlags.Shape);
                _debugView.AppendFlags(DebugViewFlags.Joint);
                _debugView.AppendFlags(DebugViewFlags.PerformanceGraph);
                _debugView.AppendFlags(DebugViewFlags.DebugPanel);
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
                LoadContent();

            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities.ElementAt(i);
                var bodyComponent = entity.GetComponent<BodyComponent>();

                bodyComponent.Enabled = entity.Active;

                // Update Entity position and rotation
                entity.SetTransform(bodyComponent.Position, MathHelper.ToDegrees(bodyComponent.Rotation));
            }

            _world.Step(Scene.DeltaTime);

            if (Scene.GameCore.DebugActive && entities.Any())
                _debugView.UpdatePerformanceGraph(_world.UpdateTime);
        }

        internal void DrawDebugData()
        {
            if (Scene.GameCore.DebugActive && Scene.ExistsEntities(_ => MatchActiveEntitiesAndComponents(_)))
                _debugView.RenderDebugData(Scene.Camera2D.Projection, Scene.Camera2D.View);
        }
    }
}
