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
        Entity _shipEntity;
        Entity _textEntity;

        public override void LoadContent()
        {
            SetTitle(nameof(S03AsteroidsMovementScene));

            var gamepadButtonsPosition = new Vector2(50, ScreenHeight - 450);
            var texture = GameCore.Content.Load<Texture2D>("Common/D-Pad");

            _touchGamepadButtonsComponent = new TouchGamepadButtonsComponent(
                GameCore,
                new GamepadButtonsConfiguration(400, gamepadButtonsPosition, texture));

            AddGameComponent(_touchGamepadButtonsComponent);

            var shipTexture = GameCore.Content.Load<Texture2D>("Sample03/Ship");

            _bodyComponent = new BodyComponent(shipTexture.Width, shipTexture.Height, EntityType.Dynamic, EntityShape.Rectangle)
            {
                IgnoreGravity = true,
            };

            _shipEntity = CreateEntity("ship", ScreenCenter)
                .AddComponent(_bodyComponent)
                .AddComponent(new SpriteComponent(shipTexture));

            var spriteFont = GameCore.Content.Load<SpriteFont>("Common/FontArial18");
            _textComponent = new TextComponent(spriteFont, "", color: Color.Black);
            _textEntity = CreateEntity("text", ScreenCenter)
                .AddComponent(_textComponent);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var rotationToVector = _shipEntity.RotationToVector();

            _textEntity.SetPosition(new Vector2(_shipEntity.Position.X, _shipEntity.Position.Y + 100f));
            _textComponent.Text = $"Rotation to Vector: {rotationToVector}" +
                $"\nButton Touched: {_touchGamepadButtonsComponent.ButtonTouched}";

            // F = m * a
            var linearImpulse = _bodyComponent.Mass * 4;

            // T = I * a
            var angularImpulse = _bodyComponent.Inertia * 0.1f;

            // Change Angle
            if (_touchGamepadButtonsComponent.IsTouched(Buttons.Button02))
            {
                _bodyComponent.ApplyAngularImpulse(-angularImpulse);
            }
            else if (_touchGamepadButtonsComponent.IsTouched(Buttons.Button03))
            {
                _bodyComponent.ApplyAngularImpulse(angularImpulse);
            }
            else
            {
                _bodyComponent.AngularVelocity = 0f;
            }

            // Change Linear Impulse
            if (_touchGamepadButtonsComponent.IsTouched(Buttons.Button01))
            {
                _bodyComponent.ApplyLinearImpulse(new Vector2(-linearImpulse * rotationToVector.X, -linearImpulse * rotationToVector.Y));
            }

            if (_touchGamepadButtonsComponent.IsTouched(Buttons.Button04))
            {
                _bodyComponent.ApplyLinearImpulse(new Vector2(linearImpulse / 4 * rotationToVector.X, linearImpulse / 4 * rotationToVector.Y));
            }

            base.Update(gameTime);
        }
    }
}
