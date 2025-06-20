using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curupira2D.GameComponents.GamepadButtons
{
    public class TouchGamepadButtonsComponent : DrawableGameComponent, IEquatable<TouchGamepadButtonsComponent>
    {
        readonly Rectangle _gamepadButtonsBoundSizeAndLocation;
        readonly Dictionary<Buttons, Rectangle> _gamepadButtons;
        readonly SpriteBatch _spriteBatch;
        readonly GamepadButtonsConfiguration _gamepadButtonsConfiguration;

        public TouchGamepadButtonsComponent(Game game, GamepadButtonsConfiguration gamepadButtonsConfiguration) : base(game)
        {
            Active = true;
            ButtonTouched = Buttons.None;

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gamepadButtonsConfiguration = gamepadButtonsConfiguration ?? throw new ArgumentNullException();
            _gamepadButtonsBoundSizeAndLocation = new Rectangle(_gamepadButtonsConfiguration.Position.ToPoint(), new Point(_gamepadButtonsConfiguration.Size));

            var gamePadButtonsSize = new Point(_gamepadButtonsConfiguration.Size / 3);

            _gamepadButtons = new Dictionary<Buttons, Rectangle>
            {
                {
                    Buttons.Button01,
                    new Rectangle(
                        new Point((int)_gamepadButtonsConfiguration.Position.X + gamePadButtonsSize.X, (int)_gamepadButtonsConfiguration.Position.Y),
                        gamePadButtonsSize)
                },
                {
                    Buttons.Button02,
                    new Rectangle(
                        new Point((int)_gamepadButtonsConfiguration.Position.X + gamePadButtonsSize.X * 2, (int)_gamepadButtonsConfiguration.Position.Y + gamePadButtonsSize.X),
                        gamePadButtonsSize)
                },
                {
                    Buttons.Button03,
                    new Rectangle(
                        new Point((int)_gamepadButtonsConfiguration.Position.X, (int)_gamepadButtonsConfiguration.Position.Y + gamePadButtonsSize.X),
                        gamePadButtonsSize)
                },
                {
                    Buttons.Button04,
                    new Rectangle(
                        new Point((int)_gamepadButtonsConfiguration.Position.X + gamePadButtonsSize.X, (int)_gamepadButtonsConfiguration.Position.Y + gamePadButtonsSize.X * 2),
                        gamePadButtonsSize)
                },
            };
        }

        public bool Active { get; private set; }
        public Buttons ButtonTouched { get; private set; }

        public override void Update(GameTime gameTime)
        {
            if (!Active)
                return;

            ButtonTouched = Buttons.None;
            var touchCollections = TouchPanel.GetState();

            if (touchCollections.Count == 0)
                return;

            var touchPositions = touchCollections.Select(_ => new Rectangle(_.Position.ToPoint(), Point.Zero));

            Parallel.ForEach(touchPositions, touchPosition =>
            {
                foreach (var button in _gamepadButtons)
                {
                    if (touchPosition.Intersects(button.Value))
                        ButtonTouched = button.Key;
                }
            });
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Active)
                return;

            _spriteBatch.Begin();

            _spriteBatch.Draw(
                   _gamepadButtonsConfiguration.Texture,
                   _gamepadButtonsBoundSizeAndLocation,
                   _gamepadButtonsConfiguration.Color * _gamepadButtonsConfiguration.Opacity);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetActive(bool active)
        {
            ButtonTouched = Buttons.None;
            Active = active;
        }

        public bool IsTouched(Buttons buttons)
            => Active && ButtonTouched != Buttons.None && ButtonTouched == buttons;

        public bool Equals(TouchGamepadButtonsComponent other)
            => other != null && other._gamepadButtonsBoundSizeAndLocation == _gamepadButtonsBoundSizeAndLocation;
    }
}
