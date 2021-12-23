using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Curupira2D.GameComponents.Joystick
{
    public class TouchJoystickComponent : DrawableGameComponent, IEquatable<TouchJoystickComponent>
    {
        readonly Rectangle _joystickBackgroundSizeAndLocation;
        readonly Rectangle _joystickHandleFallbackSizeAndLocation;
        Rectangle _joystickHandleSizeAndLocation;
        readonly SpriteBatch _spriteBatch;
        readonly Texture2D _joystickBackgroundLineTexture;
        readonly JoystickConfiguration _joystickConfiguration;

        public TouchJoystickComponent(Game game, JoystickConfiguration joystickConfiguration) : base(game)
        {
            Active = true;
            _joystickConfiguration = joystickConfiguration ?? throw new ArgumentNullException();
            _joystickBackgroundSizeAndLocation = new Rectangle(_joystickConfiguration.Position.ToPoint(), new Point(_joystickConfiguration.Size));

            var joystickHandleSizeValue = _joystickConfiguration.HandleSize == JoystickHandleSize.Large ? 1.5f : (float)_joystickConfiguration.HandleSize;
            var joystickHandleWidth = (int)(_joystickBackgroundSizeAndLocation.Width / joystickHandleSizeValue);
            var joystickHandleHeight = (int)(_joystickBackgroundSizeAndLocation.Height / joystickHandleSizeValue);
            _joystickHandleFallbackSizeAndLocation = new Rectangle(
                _joystickBackgroundSizeAndLocation.Center.X - (joystickHandleWidth / 2),
                _joystickBackgroundSizeAndLocation.Center.Y - (joystickHandleHeight / 2),
                joystickHandleWidth,
                joystickHandleHeight);

            _joystickHandleSizeAndLocation = _joystickHandleFallbackSizeAndLocation;

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            if (_joystickConfiguration.BackgroundTexture == null)
            {
                var texture = new Texture2D(GraphicsDevice, 1, 1);
                texture.SetData(new Color[] { Color.Black });

                _joystickConfiguration.BackgroundTexture = texture;

                _joystickBackgroundLineTexture = new Texture2D(GraphicsDevice, 1, 1);
                _joystickBackgroundLineTexture.SetData(new Color[] { Color.White });
            }

            if (_joystickConfiguration.HandleTexture == null)
            {
                var texture = new Texture2D(GraphicsDevice, 1, 1);
                texture.SetData(new Color[] { Color.Gray });

                _joystickConfiguration.HandleTexture = texture;
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

            var touchPositions = touchCollections.Select(_ => new Rectangle(_.Position.ToPoint(), Point.Zero));

            Parallel.ForEach(touchPositions, touchPosition =>
            {
                if (touchPosition.Intersects(_joystickBackgroundSizeAndLocation))
                {
                    var half = _joystickBackgroundSizeAndLocation.Center - _joystickBackgroundSizeAndLocation.Location;
                    var touchPositionInBound = touchPosition.Location - _joystickBackgroundSizeAndLocation.Location;
                    var direction = Vector2.Zero;

                    if (touchPositionInBound.X < half.X * 0.9f)
                        direction.X = _joystickConfiguration.InvertX_Axis ? 1 : -1; // LEFT

                    if (touchPositionInBound.Y < half.Y * 0.9f)
                        direction.Y = _joystickConfiguration.InvertY_Axis ? 1 : -1; // UP

                    if (touchPositionInBound.X > half.X * 1.1f)
                        direction.X = _joystickConfiguration.InvertX_Axis ? -1 : 1; // RIGHT

                    if (touchPositionInBound.Y > (half.Y * 1.1f))
                        direction.Y = _joystickConfiguration.InvertY_Axis ? -1 : 1; // DOWN

                    Direction = direction;

                    var posX = touchPosition.X - _joystickHandleSizeAndLocation.Width / 2;
                    var posY = touchPosition.Y - _joystickHandleSizeAndLocation.Height / 2;
                    var joystickHandleLocaltion = new Point(posX, posY);
                    _joystickHandleSizeAndLocation = new Rectangle(joystickHandleLocaltion, _joystickHandleSizeAndLocation.Size);
                }
            });
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Active)
                return;

            _spriteBatch.Begin();

            _spriteBatch.Draw(
                _joystickConfiguration.BackgroundTexture,
                _joystickBackgroundSizeAndLocation,
                Color.White * _joystickConfiguration.BackgroundOpacity);

            _spriteBatch.Draw(
                _joystickConfiguration.HandleTexture,
                _joystickHandleSizeAndLocation,
                Color.White * _joystickConfiguration.HandleOpacity);

            if (_joystickBackgroundLineTexture != null)
            {
                #region Vertical Line
                var posV1 = new Rectangle(
                    (int)(_joystickBackgroundSizeAndLocation.Center.X - _joystickBackgroundSizeAndLocation.Width * 0.165f),
                   _joystickBackgroundSizeAndLocation.Y,
                   1,
                   _joystickBackgroundSizeAndLocation.Height);
                var posV2 = new Rectangle(
                   (int)(_joystickBackgroundSizeAndLocation.Center.X + _joystickBackgroundSizeAndLocation.Width * 0.165f),
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
}
