using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Testbed.Systems.Physic
{
    [RequiredComponent(typeof(BallControllerSystem), typeof(SpriteComponent))]
    class BallControllerSystem : ECS.System, ILoadable, IUpdatable
    {
        const float IMPULSE = 1000f;
        Entity _ballEntity;

        public void LoadContent()
        {
            var ballRadius = 25;
            var ballTexture = Scene.GameCore.GraphicsDevice.CreateTextureCircle(ballRadius, Color.Black * 0.6f);

            _ballEntity = Scene.CreateEntity("ball")
                .SetPosition(Scene.ScreenWidth * 0.4f, 300)
                .AddComponent(
                    new SpriteComponent(ballTexture),
                    new BodyComponent(ballTexture.Bounds.Size.ToVector2(), EntityType.Dynamic, EntityShape.Circle)
                    {
                        Radius = ballRadius,
                        Restitution = 1f,
                        IgnoreGravity = true
                    });
        }

        public void Update()
        {
            var keyState = Keyboard.GetState();
            var ballBodyComponent = _ballEntity.GetComponent<BodyComponent>();

            var linearImpulse = Vector2.Zero;

            if (keyState.IsKeyDown(Keys.Left))
                linearImpulse += new Vector2(-IMPULSE, 0);

            if (keyState.IsKeyDown(Keys.Up))
                linearImpulse += new Vector2(0, IMPULSE);

            if (keyState.IsKeyDown(Keys.Right))
                linearImpulse += new Vector2(IMPULSE, 0);

            if (keyState.IsKeyDown(Keys.Down))
                linearImpulse += new Vector2(0, -IMPULSE);

            ballBodyComponent.ApplyLinearImpulse(linearImpulse);
        }
    }
}
