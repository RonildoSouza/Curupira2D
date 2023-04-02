using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.Extensions;
using Curupira2D.GameComponents.Joystick;
using Curupira2D.Mobile.Samples.Common.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.Mobile.Samples.Scenes
{
    public class S01JoystickScene : SceneBase
    {
        TouchJoystickComponent _touchJoystickComponent;
        Entity _playerEntity;

        public override void LoadContent()
        {
            SetTitle(nameof(S01JoystickScene));

            var touchJoystickPosition = new Vector2(50, ScreenHeight - 450);
            var joystickBackgroundTexture = GameCore.Content.Load<Texture2D>("Common/JoystickBackground");
            var joystickHandleTexture = GameCore.Content.Load<Texture2D>("Common/JoystickHandle");

            _touchJoystickComponent = new TouchJoystickComponent(
                GameCore,
                new JoystickConfiguration(400, touchJoystickPosition)
                {
                    BackgroundTexture = joystickBackgroundTexture,
                    HandleTexture = joystickHandleTexture,
                    InvertY_Axis = true,
                    HandleSize = JoystickHandleSize.Large
                });

            AddGameComponent(_touchJoystickComponent);

            var playerTexture = new Texture2D(GameCore.GraphicsDevice, 1, 1);
            playerTexture.SetData(new Color[] { Color.DodgerBlue });

            _playerEntity = CreateEntity("player", ScreenCenter)
                .AddComponent(new SpriteComponent(texture: playerTexture, scale: new Vector2(200f)));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var tempPosition = _playerEntity.Position;
            tempPosition += (float)(200f * DeltaTime) * _touchJoystickComponent.Direction.GetSafeNormalize();

            _playerEntity.SetPosition(tempPosition);

            base.Update(gameTime);
        }
    }
}
