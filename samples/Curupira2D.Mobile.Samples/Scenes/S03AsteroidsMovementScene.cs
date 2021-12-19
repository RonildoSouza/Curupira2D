using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.GameComponents.GamepadButtons;
using Curupira2D.Mobile.Samples.Common.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.Mobile.Samples.Scenes
{
    public class S03AsteroidsMovementScene : SceneBase
    {
        TouchGamepadButtonsComponent _touchGamepadButtonsComponent;
        BodyComponent _bodyComponent;
        TextComponent _textComponent;
        Entity _playerEntity;
        Entity _textEntity;

        public override void LoadContent()
        {
            SetTitle(nameof(S03AsteroidsMovementScene));

            var gamepadButtonsPosition = new Vector2(50, ScreenHeight - 450);
            var texture = new Texture2D(GameCore.GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.Black });

            _touchGamepadButtonsComponent = new TouchGamepadButtonsComponent(
                GameCore,
                new GamepadButtonsConfiguration(400, gamepadButtonsPosition, texture, texture, texture, texture));

            AddGameComponent(_touchGamepadButtonsComponent);

            var playerTexture = new Texture2D(GameCore.GraphicsDevice, 1, 1);
            playerTexture.SetData(new Color[] { Color.DarkRed });

            _bodyComponent = new BodyComponent(playerTexture.Width, playerTexture.Height, EntityType.Dynamic, EntityShape.Rectangle)
            {
                IgnoreGravity = true,
            };

            _playerEntity = CreateEntity("player", ScreenCenter)
                .AddComponent(_bodyComponent)
                .AddComponent(new SpriteComponent(texture: playerTexture, scale: new Vector2(100f, 150f)));

            var spriteFont = GameCore.Content.Load<SpriteFont>("Common/FontArial18");
            _textComponent = new TextComponent(spriteFont, "", color: Color.Black);
            _textEntity = CreateEntity("text", ScreenCenter)
                .AddComponent(_textComponent);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var rotationToVector = _playerEntity.RotationToVector();

            _textEntity.SetPosition(new Vector2(_playerEntity.Position.X, _playerEntity.Position.Y + 100f));
            _textComponent.Text = $"Rotation to Vector: {rotationToVector}" +
                $"\nButton Touched: {_touchGamepadButtonsComponent.ButtonTouched}";

            // Change Angle
            if (_touchGamepadButtonsComponent.ButtonTouched == Buttons.Button02)
            {
                _bodyComponent.ApplyAngularImpulse(-0.01f);
            }
            else if (_touchGamepadButtonsComponent.ButtonTouched == Buttons.Button03)
            {
                _bodyComponent.ApplyAngularImpulse(0.01f);
            }
            else
            {
                _bodyComponent.AngularVelocity = 0f;
            }

            // Change Linear Impulse
            if (_touchGamepadButtonsComponent.ButtonTouched == Buttons.Button01)
            {
                _bodyComponent.ApplyLinearImpulse(new Vector2(-100f * rotationToVector.X, -100f * rotationToVector.Y));
            }

            if (_touchGamepadButtonsComponent.ButtonTouched == Buttons.Button04)
            {
                _bodyComponent.ApplyLinearImpulse(new Vector2(25f * rotationToVector.X, 25f * rotationToVector.Y));
            }

            base.Update(gameTime);
        }
    }
}

