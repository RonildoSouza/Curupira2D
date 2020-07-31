using Microsoft.Xna.Framework;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Components.Physics;
using MonoGame.Helper.Extensions;
using System.Linq;
using tainicom.Aether.Physics2D.Diagnostics;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGame.Helper.ECS.Systems.Physics
{
    [RequiredComponent(typeof(BodyComponent))]
    [RequiredComponent(typeof(SpriteComponent))]
    public class PhysicsSystem : System, IInitializable, IUpdatable, IRenderable
    {
        DebugView _debugView;
        Vector2 _gravity;
        World _world;

        public PhysicsSystem(Vector2 gravity = default)
        {
            if (gravity == default)
                SetGravity(new Vector2(0f, 9.80665f));
        }

        public Vector2 Gravity
        {
            get => _gravity;
            set => SetGravity(value);
        }
        public bool DebugActive { get; set; }
        public Color DebugDefaultShapeColor { get; set; } = Color.Orange;
        public Color DebugSleepingShapeColor { get; set; } = Color.DodgerBlue;
        public Color DebugTextColor { get; set; } = Color.Black;

        public void Initialize()
        {
            _world = new World(_gravity);
            var entities = Scene.GetEntities(_ => MatchActiveEntitiesAndComponents(_));

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var bodyComponent = entity.GetComponent<BodyComponent>();
                var spriteComponent = entity.GetComponent<SpriteComponent>();
                BodyType bodyType = (BodyType)bodyComponent.EntityType;
                Body body = null;

                switch (bodyComponent.EntityShape)
                {
                    case EntityShape.Circle:
                        body = _world.CreateCircle(
                            bodyComponent.Radius,
                            bodyComponent.Density,
                            entity.Transform.Position,
                            bodyType);
                        break;
                    case EntityShape.Ellipse:
                        body = _world.CreateEllipse(
                            spriteComponent.TextureSize.X * 0.5f,
                            spriteComponent.TextureSize.Y * 0.5f,
                            1,
                            bodyComponent.Density,
                            entity.Transform.Position,
                            entity.Transform.RotationInDegrees,
                            bodyType);
                        break;
                    case EntityShape.Rectangle:
                        body = _world.CreateRectangle(
                            spriteComponent.TextureSize.X,
                            spriteComponent.TextureSize.Y,
                            bodyComponent.Density,
                            entity.Transform.Position,
                            entity.Transform.RotationInDegrees,
                            bodyType);
                        break;
                }

                if (body == null)
                    return;

                body.Tag = entity.UniqueId;
                body.IgnoreGravity = bodyComponent.IgnoreGravity;
                body.Inertia = bodyComponent.Inertia;
                body.SetRestitution(bodyComponent.Restitution);
                body.SetFriction(bodyComponent.Friction);
            };

            if (DebugActive)
            {
                _debugView = new DebugView(_world);
                _debugView.AppendFlags(DebugViewFlags.Shape);
                _debugView.AppendFlags(DebugViewFlags.Joint);
                _debugView.AppendFlags(DebugViewFlags.PerformanceGraph);
                _debugView.AppendFlags(DebugViewFlags.DebugPanel);
                _debugView.DefaultShapeColor = DebugDefaultShapeColor;
                _debugView.SleepingShapeColor = DebugSleepingShapeColor;
                _debugView.TextColor = DebugTextColor;

                _debugView.LoadContent(Scene.GameCore.GraphicsDevice, Scene.GameCore.Content);
            }
        }

        public void Update()
        {
            _world.Step(Scene.DeltaTime);

            var entities = Scene.GetEntities(_ => MatchActiveEntitiesAndComponents(_));

            if (entities.Count != _world.BodyList.Count)
                Initialize();

            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities.ElementAt(i);
                var bodyComponent = entity.GetComponent<BodyComponent>();
                var spriteComponent = entity.GetComponent<SpriteComponent>();
                var body = _world.BodyList.GetEntityBody(entity);

                if (body == null)
                    continue;

                body.Enabled = entity.Active;
                body.ApplyForce(bodyComponent.Force);
                body.ApplyTorque(bodyComponent.Torque);
                body.ApplyLinearImpulse(bodyComponent.LinearImpulse);
                body.ApplyAngularImpulse(bodyComponent.AngularImpulse);

                // Update MonoGame.Helper.ECS.Entity position and rotation
                entity.SetTransform(body.Position, MathHelper.ToDegrees(body.Rotation));

                // Update MonoGame.Helper.ECS.Entity component
                bodyComponent.Inertia = body.Inertia;
                entity.UpdateComponent(bodyComponent);
            }

            if (DebugActive)
                _debugView.UpdatePerformanceGraph(_world.UpdateTime);
        }

        public void Draw()
        {
            if (DebugActive)
            {
                var matrix = Matrix.CreateOrthographicOffCenter(
                0f,
                Scene.GameCore.GraphicsDevice.Viewport.Width,
                Scene.GameCore.GraphicsDevice.Viewport.Height,
                0f, 0f, -1f);

                _debugView.RenderDebugData(matrix, Matrix.Identity, null, null, null, null, 1f);
            }
        }

        void SetGravity(Vector2 gravity)
        {
            if (_world == null)
                _gravity = gravity;
            else
                _world.Gravity = gravity;
        }
    }
}
