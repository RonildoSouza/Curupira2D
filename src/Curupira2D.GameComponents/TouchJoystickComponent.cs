using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Linq;

namespace Curupira2D.GameComponents
{
    public class TouchJoystickComponent : DrawableGameComponent, IEquatable<TouchJoystickComponent>
    {
        readonly Rectangle _joystickBackgroundSizeAndLocation;
        readonly Rectangle _joystickHandleFallbackSizeAndLocation;
        Rectangle _joystickHandleSizeAndLocation;
        readonly SpriteBatch _spriteBatch;
        readonly Joystick _joystickBackground;
        readonly Joystick _joystickHandle;
        readonly Texture2D _joystickBackgroundLineTexture;

        public TouchJoystickComponent(Game game, int size, Vector2 position, Joystick joystickBackground = null, Joystick joystickHandle = null) : base(game)
        {
            Active = true;
            _joystickBackgroundSizeAndLocation = new Rectangle(position.ToPoint(), new Point(size));

            var joystickButtonWidth = _joystickBackgroundSizeAndLocation.Width / 2;
            var joystickButtonHeight = _joystickBackgroundSizeAndLocation.Height / 2;
            _joystickHandleFallbackSizeAndLocation = new Rectangle(
                _joystickBackgroundSizeAndLocation.Center.X - (joystickButtonWidth / 2),
                _joystickBackgroundSizeAndLocation.Center.Y - (joystickButtonHeight / 2),
                joystickButtonWidth,
                joystickButtonHeight);

            _joystickHandleSizeAndLocation = _joystickHandleFallbackSizeAndLocation;

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _joystickBackground = joystickBackground;
            _joystickHandle = joystickHandle;

            if (_joystickBackground == null || _joystickBackground.Texture == null)
            {
                var texture = new Texture2D(GraphicsDevice, 1, 1);
                texture.SetData(new Color[] { Color.Black });

                _joystickBackground = new Joystick(texture);

                _joystickBackgroundLineTexture = new Texture2D(GraphicsDevice, 1, 1);
                _joystickBackgroundLineTexture.SetData(new Color[] { Color.White });
            }

            if (_joystickHandle == null || _joystickHandle.Texture == null)
            {
                var texture = new Texture2D(GraphicsDevice, 1, 1);
                texture.SetData(new Color[] { Color.Gray });

                _joystickHandle = new Joystick(texture);
            }

            Direction = Vector2.Zero;
        }

        public bool Active { get; private set; }
        public Vector2 Direction { get; private set; }

        public override void Update(GameTime gameTime)
        {
            if (!Active)
                return;

            var touchCollections = TouchPanel.GetState();

            if (!touchCollections.Any())
            {
                Direction = Vector2.Zero;
                _joystickHandleSizeAndLocation = _joystickHandleFallbackSizeAndLocation;
                return;
            }

            var touch = touchCollections.FirstOrDefault();
            var touchPosition = new Rectangle(touch.Position.ToPoint(), Point.Zero);

            if (touchPosition.Intersects(_joystickBackgroundSizeAndLocation))
            {
                var half = _joystickBackgroundSizeAndLocation.Center - _joystickBackgroundSizeAndLocation.Location;
                var touchPositionInBound = touchPosition.Location - _joystickBackgroundSizeAndLocation.Location;
                var direction = Vector2.Zero;

                if (touchPositionInBound.X < (half.X * 0.66f))
                    direction.X = -1; // LEFT

                if (touchPositionInBound.Y < (half.Y * 0.66f))
                    direction.Y = -1; // UP

                if (touchPositionInBound.X > (half.X * 1.33f))
                    direction.X = 1; // RIGHT

                if (touchPositionInBound.Y > (half.Y * 1.33f))
                    direction.Y = 1; // DOWN

                Direction = direction;

                var posX = touchPosition.X - _joystickHandleSizeAndLocation.Width / 2;
                _joystickHandleSizeAndLocation = new Rectangle(
                    new Point(posX, _joystickHandleSizeAndLocation.Location.Y),
                    _joystickHandleSizeAndLocation.Size);

                var posY = touchPosition.Y - _joystickHandleSizeAndLocation.Height / 2;
                _joystickHandleSizeAndLocation = new Rectangle(
                    new Point(_joystickHandleSizeAndLocation.Location.X, posY),
                    _joystickHandleSizeAndLocation.Size);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Active)
                return;

            _spriteBatch.Begin();

            _spriteBatch.Draw(
                _joystickBackground.Texture,
                _joystickBackgroundSizeAndLocation,
                Color.White * _joystickBackground.Opacity);

            _spriteBatch.Draw(
                _joystickHandle.Texture,
                _joystickHandleSizeAndLocation,
                Color.White * _joystickHandle.Opacity);

            if (_joystickBackgroundLineTexture != null)
            {
                #region Vertical Line
                var posV1 = new Rectangle(
                   (int)(_joystickBackgroundSizeAndLocation.Center.X - _joystickBackgroundSizeAndLocation.Center.X * 0.24f),
                   _joystickBackgroundSizeAndLocation.Y,
                   1,
                   _joystickBackgroundSizeAndLocation.Height);
                var posV2 = new Rectangle(
                   (int)(_joystickBackgroundSizeAndLocation.Center.X + _joystickBackgroundSizeAndLocation.Center.X * 0.24f),
                   _joystickBackgroundSizeAndLocation.Y,
                   1,
                   _joystickBackgroundSizeAndLocation.Height);

                _spriteBatch.Draw(_joystickBackgroundLineTexture, posV1, Color.White);
                _spriteBatch.Draw(_joystickBackgroundLineTexture, posV2, Color.White);
                #endregion

                #region Horizontal Line
                var posH1 = new Rectangle(
                   _joystickBackgroundSizeAndLocation.X,
                   (int)(_joystickBackgroundSizeAndLocation.Center.Y - _joystickBackgroundSizeAndLocation.Height * 0.165f),
                   _joystickBackgroundSizeAndLocation.Width,
                   1);
                var posH2 = new Rectangle(
                   _joystickBackgroundSizeAndLocation.X,
                   (int)(_joystickBackgroundSizeAndLocation.Center.Y + _joystickBackgroundSizeAndLocation.Height * 0.165f),
                   _joystickBackgroundSizeAndLocation.Width,
                   1);

                _spriteBatch.Draw(_joystickBackgroundLineTexture, posH1, Color.White);
                _spriteBatch.Draw(_joystickBackgroundLineTexture, posH2, Color.White);
                #endregion
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetActive(bool active)
        {
            Direction = Vector2.Zero;
            _joystickHandleSizeAndLocation = _joystickHandleFallbackSizeAndLocation;
            Active = active;
        }

        public bool Equals(TouchJoystickComponent other)
            => other != null && other._joystickBackgroundSizeAndLocation == _joystickBackgroundSizeAndLocation;
    }

    public class Joystick
    {
        public Joystick(Texture2D texture, float opacity = 0.6f)
        {
            Texture = texture;
            Opacity = opacity;
        }

        public Texture2D Texture { get; private set; }
        public float Opacity { get; private set; }
    }
}
