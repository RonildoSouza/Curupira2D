using Microsoft.Xna.Framework;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using MonoGame.Helper.Physic.Components;
using MonoGame.Helper.Physic.Extensions;
using System.Linq;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGame.Helper.Physic.Systems
{
    [RequiredComponent(typeof(BodyComponent))]
    [RequiredComponent(typeof(SpriteComponent))]
    public class AetherPhysics2DSystem : SystemPhysics, IInitializable, IUpdatable
    {
        public void Initialize()
        {
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
                        body = Scene.World.CreateCircle(
                            bodyComponent.Radius,
                            bodyComponent.Density,
                            entity.Transform.Position,
                            bodyType);
                        break;
                    case EntityShape.Rectangle:
                        body = Scene.World.CreateRectangle(
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
        }

        public void Update()
        {
            Scene.World.Step(Scene.DeltaTime);

            var entities = Scene.GetEntities(_ => MatchActiveEntitiesAndComponents(_));

            if (entities.Count != Scene.World.BodyList.Count)
                Initialize();

            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities.ElementAt(i);
                var bodyComponent = entity.GetComponent<BodyComponent>();
                var spriteComponent = entity.GetComponent<SpriteComponent>();
                var body = Scene.World.BodyList.GetEntityBody(entity);

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
        }
    }
}
