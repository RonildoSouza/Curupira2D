using Curupira2D.ECS;
using Curupira2D.GameComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.Mobile.Samples
{
    public class Game1 : GameCore
    {
        public Game1() : base(debugActive: true)
        {
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            SetScene<MenuScene>();
            base.LoadContent();
        }
    }

    public class MenuScene : Scene
    {
        TouchJoystickComponent _touchJoystickComponent;
        Texture2D _playerTexture;
        Vector2 _position;

        public override void LoadContent()
        {
            var touchJoystickPosition = new Vector2(50, ScreenHeight - 450);

            var joystickBackgroundTexture = GameCore.Content.Load<Texture2D>("JoystickBackground");
            var joystickHandleTexture = GameCore.Content.Load<Texture2D>("JoystickHandle");

            _touchJoystickComponent = new TouchJoystickComponent(
                GameCore, 
                400, 
                touchJoystickPosition,
                new Joystick(joystickBackgroundTexture),
                new Joystick(joystickHandleTexture));

            GameCore.Components.Add(_touchJoystickComponent);

            _playerTexture = new Texture2D(GameCore.GraphicsDevice, 1, 1);
            _playerTexture.SetData(new Color[] { Color.DodgerBlue });

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _position += (new Vector2(100f) * deltaTime) * _touchJoystickComponent.Direction;

            base.Update(gameTime);
        }

        public override void Draw()
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(_playerTexture, new Rectangle(_position.ToPoint(), new Point(200)), Color.White);
            SpriteBatch.End();

            base.Draw();
        }
    }
}
