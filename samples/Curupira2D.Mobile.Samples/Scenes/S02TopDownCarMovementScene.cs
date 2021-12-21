using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.GameComponents.Joystick;
using Curupira2D.Mobile.Samples.Common.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.Mobile.Samples.Scenes
{
    public class S02TopDownCarMovementScene : SceneBase
    {
        TouchJoystickComponent _touchJoystickComponent;
        BodyComponent _bodyComponent;
        TextComponent _textComponent;
        Entity _carEntity;
        Entity _textEntity;

        public override void LoadContent()
        {
            SetTitle(nameof(S02TopDownCarMovementScene));

            var joystickPosition = new Vector2(50, ScreenHeight - 450);
            var joystickBackgroundTexture = GameCore.Content.Load<Texture2D>("Common/JoystickBackground");
            var joystickHandleTexture = GameCore.Content.Load<Texture2D>("Common/JoystickHandle");

            _touchJoystickComponent = new TouchJoystickComponent(
                GameCore,
                new JoystickConfiguration(400, joystickPosition)
                {
                    BackgroundTexture = joystickBackgroundTexture,
                    HandleTexture = joystickHandleTexture,
                });

            AddGameComponent(_touchJoystickComponent);

            var carTexture = GameCore.Content.Load<Texture2D>("Sample02/Car");

            _bodyComponent = new BodyComponent(carTexture.Width, carTexture.Height, EntityType.Dynamic, EntityShape.Rectangle)
            {
                IgnoreGravity = true,
            };

            _carEntity = CreateEntity("player", ScreenCenter)
                .AddComponent(_bodyComponent)
                .AddComponent(new SpriteComponent(texture: carTexture));

            var spriteFont = GameCore.Content.Load<SpriteFont>("Common/FontArial18");
            _textComponent = new TextComponent(spriteFont, "", color: Color.Black);
            _textEntity = CreateEntity("text", ScreenCenter)
                .AddComponent(_textComponent);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var rotationToVector = _carEntity.RotationToVector();

            // F = m * a
            var linearImpulse = _bodyComponent.Mass * 4;

            // T = I * a
            var angularImpulse = _bodyComponent.Inertia * 0.1f;

            _textEntity.SetPosition(new Vector2(_carEntity.Position.X, _carEntity.Position.Y + 150f));
            _textComponent.Text = $"Rotation to Vector: {rotationToVector}" +
                $"\nAngular Velocity: {_bodyComponent.AngularVelocity}" +
                $"\nLinear Velocity: {_bodyComponent.LinearVelocity}";

            // Change Angle
            if (_touchJoystickComponent.Direction.X != 0)
            {
                if ((_touchJoystickComponent.Direction.X == -1 && _bodyComponent.AngularVelocity < 0.5f)
                    || (_touchJoystickComponent.Direction.X == 1 && _bodyComponent.AngularVelocity > -0.5f))
                    _bodyComponent.ApplyAngularImpulse(_touchJoystickComponent.Direction.X * -angularImpulse);
            }
            else
            {
                _bodyComponent.AngularVelocity = 0f;
            }

            // Change Linear Impulse
            if (_touchJoystickComponent.Direction.Y != 0)
            {
                _bodyComponent.ApplyLinearImpulse(new Vector2(
                    _touchJoystickComponent.Direction.Y * linearImpulse * rotationToVector.X,
                    _touchJoystickComponent.Direction.Y * linearImpulse * rotationToVector.Y));
            }
            else
            {
                _bodyComponent.LinearVelocity = Vector2.Zero;
            }

            base.Update(gameTime);
        }
    }
}

