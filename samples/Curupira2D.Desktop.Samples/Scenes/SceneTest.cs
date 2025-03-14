using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Desktop.Samples.Scenes
{
    /// <summary>
    /// https://www.iforce2d.net/b2dtut/introduction
    /// </summary>
    class SceneTest : SceneBase
    {
        Vector2 _cameraPosition;
        BodyComponent _playerBodyComponent;
        TextComponent _textComponent;
        float _impulse = 0f;
        bool _isMoving;

        public override void LoadContent()
        {
            Gravity = new Vector2(0f, Gravity.Y * 21);

            var playerSpriteComponent = new SpriteComponent(GameCore.Content.Load<Texture2D>("_test_/3d-woman"));
            _playerBodyComponent = new BodyComponent(playerSpriteComponent.TextureSize * playerSpriteComponent.Scale, EntityType.Dynamic, EntityShape.Rectangle)
            {
                FixedRotation = true,
            };

            CreateEntity("player", ScreenWidth * 0.5f, ScreenHeight * 0.2f)
                .AddComponent(playerSpriteComponent)
                .AddComponent(_playerBodyComponent);

            var groundSpriteComponent = new SpriteComponent(GameCore.GraphicsDevice.CreateTextureRectangle(ScreenWidth, 20, Color.Black));
            CreateEntity("ground", ScreenWidth * 0.5f, groundSpriteComponent.TextureSize.Y * 0.5f)
                .AddComponent(groundSpriteComponent)
                .AddComponent(new BodyComponent(groundSpriteComponent.TextureSize, EntityType.Static, EntityShape.Rectangle));

            _textComponent = ShowText(BuildDebugText(), y: 120f);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            MouseInputManager.Begin();
            KeyboardInputManager.Begin();

            // Change Gravity
            if (KeyboardInputManager.IsKeyPressed(Keys.VolumeUp))
                Gravity += new Vector2(0f, -5);
            if (KeyboardInputManager.IsKeyPressed(Keys.VolumeDown))
                Gravity -= new Vector2(0f, -5);

            // Change Impulse
            if (KeyboardInputManager.IsKeyPressed(Keys.Up))
                _impulse += 10f;
            if (KeyboardInputManager.IsKeyPressed(Keys.Down))
                _impulse -= 10f;

            if (_impulse == 0f)
                _impulse = _playerBodyComponent.Mass * 10 / DeltaTime;

            #region Camera
            _cameraPosition.X = MouseInputManager.GetPosition().X;
            _cameraPosition.Y = InvertPositionY(MouseInputManager.GetPosition().Y);

            Camera2D.Position = _cameraPosition;

            if (KeyboardInputManager.IsKeyDown(Keys.OemPlus))
                Camera2D.Zoom -= new Vector2(0.01f);

            if (KeyboardInputManager.IsKeyDown(Keys.OemMinus))
                Camera2D.Zoom += new Vector2(0.01f);

            if (KeyboardInputManager.IsKeyPressed(Keys.R))
                Camera2D.Reset();
            #endregion

            #region Player
            if (KeyboardInputManager.IsKeyDown(Keys.Right))
            {
                _playerBodyComponent.SetLinearVelocityX(100f);
                _isMoving = true;
            }

            if (KeyboardInputManager.IsKeyDown(Keys.Left))
            {
                _playerBodyComponent.SetLinearVelocityX(-100f);
                _isMoving = true;
            }

            if (!_isMoving)
            {
                _playerBodyComponent.SetLinearVelocityX(0f);
                _impulse = _playerBodyComponent.Mass * 10 / DeltaTime;
            }

            if (KeyboardInputManager.IsKeyPressed(Keys.Space))
                _playerBodyComponent.ApplyLinearImpulseY(_impulse);
            #endregion

            KeyboardInputManager.End();
            MouseInputManager.End();

            _textComponent.Text = BuildDebugText();
            _isMoving = false;

            base.Update(gameTime);
        }

        private string BuildDebugText()
            => $"Gravity: {Gravity}\n" +
               $"Y Impulse: {_impulse}\n" +
               $"Player Mass: {_playerBodyComponent.Mass}\n" +
               $"Player Size: {_playerBodyComponent.Size}\n" +
               $"Player Position: {Vector2.Round(_playerBodyComponent.Position)}";
    }
}
