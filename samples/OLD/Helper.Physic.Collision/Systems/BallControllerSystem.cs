using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Components.Physics;
using MonoGame.Helper.ECS.Systems;
using MonoGame.Helper.Extensions;

namespace Collision.Systems
{
    [RequiredComponent(typeof(SpriteComponent))]
    public class BallControllerSystem : MonoGame.Helper.ECS.System, IInitializable, IUpdatable
    {
        const float IMPULSE = 1000f;
        Entity _ballEntity;

        public void Initialize()
        {
            var ballRadius = 25;
            var ballTexture = Scene.GameCore.GraphicsDevice.CreateTextureCircle(ballRadius, Color.Black * 0.6f);

            _ballEntity = Scene.CreateEntity("ball")
                .SetPosition(Scene.ScreenWidth * 0.4f, 300)
                .AddComponent(new SpriteComponent(ballTexture))
                .AddComponent(new BodyComponent(ballTexture.Bounds.Size.ToVector2())
                {
                    Radius = ballRadius,
                    EntityType = EntityType.Dynamic,
                    EntityShape = EntityShape.Circle,
                    Restitution = 1f,
                    IgnoreGravity = true,
                    AngularImpulse = IMPULSE,
                });
        }

        public void Update()
        {
            var ks = Keyboard.GetState();
            var ballBodyComponent = _ballEntity.GetComponent<BodyComponent>();

            var linearImpulse = Vector2.Zero;

            if (ks.IsKeyDown(Keys.Left))
                linearImpulse += new Vector2(-IMPULSE, 0);

            if (ks.IsKeyDown(Keys.Up))
                linearImpulse += new Vector2(0, -IMPULSE);

            if (ks.IsKeyDown(Keys.Right))
                linearImpulse += new Vector2(IMPULSE, 0);

            if (ks.IsKeyDown(Keys.Down))
                linearImpulse += new Vector2(0, IMPULSE);

            ballBodyComponent.LinearImpulse = linearImpulse;
        }
    }
}
