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

                var bodyPosition = entity.Transform.Position + spriteComponent.TextureOrigin;
                //bodyPosition.X -= bodyComponent.Radius * 0.5f;
                bodyPosition.Y -= bodyComponent.Radius * 0.5f;

                switch (bodyComponent.EntityShape)
                {
                    case EntityShape.Circle:
                        body = _world.CreateCircle(
                            bodyComponent.Radius,
                            bodyComponent.Density,
                            bodyPosition,
                            bodyType);
                        break;
                    case EntityShape.Rectangle:
                        body = _world.CreateRectangle(
                            spriteComponent.TextureSize.X,
                            spriteComponent.TextureSize.Y,
                            bodyComponent.Density,
                            bodyPosition,
                            entity.Transform.RotationInDegrees,
                            bodyType);
                        break;
                        //case EntityShape.Polygon:
                        //    var rect = PolygonTools.CreateRectangle(1f / 2f, 1f / 2f);
                        //    var shape = new PolygonShape(rect, bodyComponent.Density);

                        //    body = _world.CreatePolygon(rect, )
                        //    break;
                }

                if (body == null)
                    continue;

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
                var body = _world.BodyList[i];
                var bodyComponent = entity.GetComponent<BodyComponent>();
                var spriteComponent = entity.GetComponent<SpriteComponent>();

                body.ApplyForce(bodyComponent.Force);
                body.ApplyTorque(bodyComponent.Torque);
                body.ApplyLinearImpulse(bodyComponent.LinearImpulse);
                body.ApplyAngularImpulse(bodyComponent.AngularImpulse);

                // Update MonoGame.Helper.ECS.Entity position and rotation
                var newEntityPosition = body.Position - spriteComponent.TextureOrigin;
                //newEntityPosition.X += bodyComponent.Radius * 0.5f;
                newEntityPosition.Y += bodyComponent.Radius * 0.5f;
                entity.SetTransform(newEntityPosition, body.Rotation);


                // Update tainicom.Aether.Physics2D.Dynamics.Body position and rotation
                var newBodyPosition = entity.Transform.Position + spriteComponent.TextureOrigin;
                //newBodyPosition.X -= bodyComponent.Radius * 0.5f;
                newBodyPosition.Y -= bodyComponent.Radius * 0.5f;
                body.SetTransform(newBodyPosition, entity.Transform.RotationInDegrees);

                //// Update MonoGame.Helper.ECS.Entity component
                bodyComponent.Inertia = body.Inertia;
                bodyComponent.SetTransform(newBodyPosition, body.Rotation);
                entity.UpdateComponent(bodyComponent);
            }

            if (Scene.Gravity != _world.Gravity)
                _world.Gravity = Scene.Gravity;
        }
    }
}
