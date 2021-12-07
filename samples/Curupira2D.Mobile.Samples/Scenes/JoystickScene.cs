using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.Extensions;
using Curupira2D.GameComponents.Joystick;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.Mobile.Samples.Scenes
{
    public class JoystickScene : Scene
    {
        TouchJoystickComponent _touchJoystickComponent;
        Entity playerEntity;

        public override void LoadContent()
        {
            SetTitle(nameof(JoystickScene));

            var touchJoystickPosition = new Vector2(50, ScreenHeight - 450);
            var joystickBackgroundTexture = GameCore.Content.Load<Texture2D>("JoystickBackground");
            var joystickHandleTexture = GameCore.Content.Load<Texture2D>("JoystickHandle");

            _touchJoystickComponent = new TouchJoystickComponent(
                GameCore,
                new JoystickConfiguration(400, touchJoystickPosition)
                {
                    InvertY_Axis = true
                },
                new JoystickTexture(joystickBackgroundTexture),
                new JoystickTexture(joystickHandleTexture));

            GameCore.Components.Add(_touchJoystickComponent);

            var playerTexture = new Texture2D(GameCore.GraphicsDevice, 1, 1);
            playerTexture.SetData(new Color[] { Color.DodgerBlue });

            playerEntity = CreateEntity("player", ScreenCenter)
                .AddComponent(new SpriteComponent(texture: playerTexture, scale: new Vector2(200f)));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var tempPosition = playerEntity.Position;
            tempPosition += (float)(200f * DeltaTime) * _touchJoystickComponent.Direction.GetSafeNormalize();

            playerEntity.SetPosition(tempPosition);

            base.Update(gameTime);
        }
    }
}
