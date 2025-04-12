using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Desktop.Samples.Systems.Physic
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

            _ballEntity = Scene.CreateEntity("ball", Scene.ScreenWidth * 0.4f, 300f)
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
            var ballBodyComponent = _ballEntity.GetComponent<BodyComponent>();

            var linearImpulse = Vector2.Zero;

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Left) || Scene.KeyboardInputManager.IsKeyDown(Keys.A))
                linearImpulse += new Vector2(-IMPULSE, 0);

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Up) || Scene.KeyboardInputManager.IsKeyDown(Keys.W))
                linearImpulse += new Vector2(0, IMPULSE);

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Right) || Scene.KeyboardInputManager.IsKeyDown(Keys.D))
                linearImpulse += new Vector2(IMPULSE, 0);

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Down) || Scene.KeyboardInputManager.IsKeyDown(Keys.S))
                linearImpulse += new Vector2(0, -IMPULSE);

            ballBodyComponent.ApplyLinearImpulse(linearImpulse);
        }
    }
}
