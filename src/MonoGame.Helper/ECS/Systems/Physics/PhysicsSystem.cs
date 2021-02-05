using Microsoft.Xna.Framework;
using MonoGame.Helper.ECS.Components.Physics;
using MonoGame.Helper.ECS.Systems.Attributes;
using MonoGame.Helper.Extensions;
using System.Linq;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Diagnostics;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGame.Helper.ECS.Systems.Physics
{
    [RequiredComponent(typeof(PhysicsSystem), typeof(BodyComponent))]
    public class PhysicsSystem : System, IInitializable, IUpdatable, IRenderable
    {
        DebugView _debugView;

        public Color DebugDefaultShapeColor { get; set; } = Color.Orange;
        public Color DebugSleepingShapeColor { get; set; } = Color.DodgerBlue;
        public Color DebugTextColor { get; set; } = Color.Black;

        public void Initialize()
        {
            var entities = Scene.GetEntities(_ => MatchActiveEntitiesAndComponents(_));

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var bodyComponent = entity.GetComponent<BodyComponent>();
                BodyType bodyType = (BodyType)bodyComponent.EntityType;
                Body body = null;

                switch (bodyComponent.EntityShape)
                {
                    case EntityShape.Circle:
                        body = Scene.World.CreateCircle(
                            bodyComponent.Radius,
                            bodyComponent.Density,
                            entity.Transform.Position,
                            bodyType);
                        break;
                    case EntityShape.Ellipse:
                        body = Scene.World.CreateEllipse(
                            bodyComponent.Size.X * 0.5f,
                            bodyComponent.Size.Y * 0.5f,
                            8,
                            bodyComponent.Density,
                            entity.Transform.Position,
                            entity.Transform.Rotation,
                            bodyType);
                        break;
                    case EntityShape.Rectangle:
                        body = Scene.World.CreateRectangle(
                            bodyComponent.Size.X,
                            bodyComponent.Size.Y,
                            bodyComponent.Density,
                            entity.Transform.Position,
                            entity.Transform.Rotation,
                            bodyType);
                        break;
                    case EntityShape.Polygon:
                        var vertices = new Vertices(bodyComponent.Vertices);
                        body = Scene.World.CreatePolygon(
                            vertices,
                            bodyComponent.Density,
                            entity.Transform.Position,
                            entity.Transform.Rotation,
                            bodyType);
                        break;
                }

                if (body == null)
                    return;

                body.Tag = entity.UniqueId;
                body.SetRestitution(bodyComponent.Restitution);
                body.SetFriction(bodyComponent.Friction);
            };

            if (Scene.GameCore.DebugActive && entities.Any())
            {
                _debugView = new DebugView(Scene.World);
                _debugView.AppendFlags(DebugViewFlags.Shape);
                _debugView.AppendFlags(DebugViewFlags.Joint);
                _debugView.AppendFlags(DebugViewFlags.PerformanceGraph);
                _debugView.AppendFlags(DebugViewFlags.DebugPanel);
                _debugView.DefaultShapeColor = DebugDefaultShapeColor;
                _debugView.SleepingShapeColor = DebugSleepingShapeColor;
                _debugView.TextColor = DebugTextColor;
                _debugView.StaticShapeColor = Color.Red;

                _debugView.LoadContent(Scene.GameCore.GraphicsDevice, Scene.GameCore.Content);
            }
        }

        public void Update()
        {
            var entities = Scene.GetEntities(_ => MatchActiveEntitiesAndComponents(_));

            if (entities.Count != Scene.World.BodyList.Count)
            {
                Scene.World.BodyList.Clear();
                Initialize();
            }

            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities.ElementAt(i);
                var bodyComponent = entity.GetComponent<BodyComponent>();
                var body = Scene.World.BodyList.GetEntityBody(entity);

                if (body == null)
                    continue;

                body.Enabled = entity.Active;
                body.IgnoreGravity = bodyComponent.IgnoreGravity;
                body.FixedRotation = bodyComponent.FixedRotation;

                if (bodyComponent.Inertia != null)
                    body.Inertia = bodyComponent.Inertia.Value;

                if (bodyComponent.Mass != null)
                    body.Mass = bodyComponent.Mass.Value;

                body.ApplyForce(bodyComponent.Force);
                body.ApplyTorque(bodyComponent.Torque);
                body.ApplyLinearImpulse(bodyComponent.LinearImpulse);
                body.ApplyAngularImpulse(bodyComponent.AngularImpulse);

                if (bodyComponent.LinearVelocity != Vector2.Zero)
                {
                    if (bodyComponent.LinearVelocity.X != body.LinearVelocity.X)
                        body.LinearVelocity = new Vector2(bodyComponent.LinearVelocity.X, body.LinearVelocity.Y);

                    if (bodyComponent.LinearVelocity.Y != body.LinearVelocity.Y)
                        body.LinearVelocity = new Vector2(body.LinearVelocity.X, bodyComponent.LinearVelocity.Y);
                }

                // Update MonoGame.Helper.ECS.Entity position and rotation
                entity.SetTransform(body.Position, MathHelper.ToDegrees(body.Rotation));

                // Update MonoGame.Helper.ECS.Entity component
                bodyComponent.Inertia = body.Inertia;
                entity.UpdateComponent(bodyComponent);
            }

            if (Scene.GameCore.DebugActive && entities.Any())
                _debugView.UpdatePerformanceGraph(Scene.World.UpdateTime);
        }

        public void Draw()
        {
            var entities = Scene.GetEntities(_ => MatchActiveEntitiesAndComponents(_));

            if (Scene.GameCore.DebugActive && entities.Any())
                _debugView.RenderDebugData(Scene.Camera2D.Projection, Scene.Camera2D.View);
        }
    }
}
