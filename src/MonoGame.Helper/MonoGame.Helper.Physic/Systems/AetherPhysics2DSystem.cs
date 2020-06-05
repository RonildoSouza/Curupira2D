using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using MonoGame.Helper.Physic.Components;
using System.Linq;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGame.Helper.Physic.Systems
{
    [RequiredComponent(typeof(BodyComponent))]
    [RequiredComponent(typeof(SpriteComponent))]
    public class AetherPhysics2DSystem : ECS.System, IInitializable, IUpdatable
    {
        private static World _world;

        public void Initialize()
        {
            if (_world == null)
                _world = new World(Scene.Gravity);

            _world.Clear();

            var entities = Scene.GetEntities(_ => Matches(_));

            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities.ElementAt(i);
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
                    continue;

                body.Tag = entity.UniqueId;
                body.IgnoreGravity = bodyComponent.IgnoreGravity;
                body.Inertia = bodyComponent.Inertia;
                body.SetRestitution(bodyComponent.Restitution);
                body.SetFriction(bodyComponent.Friction);
            }
        }

        public void Update()
        {
            _world.Step(Scene.DeltaTime);

            var entities = Scene.GetEntities(_ => Matches(_));

            if (entities.Count != _world.BodyList.Count)
                Initialize();

            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities.ElementAt(i);
                var bodyComponent = entity.GetComponent<BodyComponent>();
                var spriteComponent = entity.GetComponent<SpriteComponent>();
                var body = _world.BodyList.FirstOrDefault(_ => _.Tag is string && (string)_.Tag == entity.UniqueId);

                if (body == null)
                    continue;

                body.ApplyForce(bodyComponent.Force);
                body.ApplyTorque(bodyComponent.Torque);
                body.ApplyLinearImpulse(bodyComponent.LinearImpulse);
                body.ApplyAngularImpulse(bodyComponent.AngularImpulse);

                // Update MonoGame.Helper.ECS.Entity position and rotation
                entity.SetTransform(body.Position, body.Rotation);

                // Update MonoGame.Helper.ECS.Entity component
                bodyComponent.Inertia = body.Inertia;
                entity.UpdateComponent(bodyComponent);
            }

            if (Scene.Gravity != _world.Gravity)
                _world.Gravity = Scene.Gravity;
        }
    }
}
