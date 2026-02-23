/*
 * https://stackoverflow.com/questions/712296/xna-2d-camera-engine-that-follows-sprite
 * http://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.GameComponents.Camera2D
{
    public class Camera2DComponent : GameComponent, ICamera2D
    {
        private Matrix _transformationMatrix = Matrix.Identity;
        private Matrix _inverseMatrix = Matrix.Identity;
        private Vector2 _position = Vector2.Zero;
        private float _rotation = 0;
        private float _zoom = 1f;
        private Vector2 _origin = Vector2.Zero;
        private bool _hasChanged;

        public Camera2DComponent(Game game) : base(game)
        {
            Viewport = game.GraphicsDevice.Viewport;
            SpriteBatchEffect = new(game.GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = true
            };
        }

        public Viewport Viewport { get; set; }
        public Matrix TransformationMatrix => _transformationMatrix;
        public Matrix InverseMatrix => _inverseMatrix;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position == value)
                    return;

                _position = value;
                _hasChanged = true;
            }
        }
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                if (_rotation == value)
                    return;

                _rotation = value;
                _hasChanged = true;
            }
        }
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                // Negative zoom will flip image
                if (_zoom == value || value <= 0f)
                    return;

                _zoom = value;
                _hasChanged = true;
            }
        }
        public Vector2 Origin
        {
            get { return _origin; }
            set
            {
                if (_origin == value)
                    return;

                _origin = value;
                _hasChanged = true;
            }
        }
        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }
        public BasicEffect SpriteBatchEffect { get; private set; }

        public override void Update(GameTime gameTime)
        {
            if (_hasChanged)
            {
                UpdateMatrices();
                UpdateProjection();
                UpdateView();

                SpriteBatchEffect.View = View;
                SpriteBatchEffect.Projection = Projection;

                _hasChanged = false;
            }

            base.Update(gameTime);
        }

        public Vector2 ScreenToWorld(Vector2 position) => Vector2.Transform(position, InverseMatrix);

        public Vector2 ScreenToWorld(float x, float y) => ScreenToWorld(new Vector2(x, y));

        public Vector2 WorldToScreen(Vector2 position) => Vector2.Transform(position, TransformationMatrix);

        public Vector2 WorldToScreen(float x, float y) => WorldToScreen(new Vector2(x, y));

        public bool IsInView(Vector2 position, int width, int height)
        {
            // If the object is not within the horizontal bounds of the screen
            if ((position.X + width) < (Position.X - Origin.X) || (position.X - width) > (Position.X + Origin.X))
                return false;

            // If the object is not within the vertical bounds of the screen
            if ((position.Y + height) < (Position.Y - Origin.Y) || (position.Y - height) > (Position.Y + Origin.Y))
                return false;

            // In View
            return true;
        }

        public bool IsInView(float x, float y, int width, int height) => IsInView(new Vector2(x, y), width, height);

        public bool IsInView(Vector2 position, Texture2D texture) => IsInView(position, texture.Width, texture.Height);

        public bool IsInView(float x, float y, Texture2D texture) => IsInView(new Vector2(x, y), texture);

        public void Reset()
        {
            _position = Vector2.Zero;
            _rotation = 0;
            _zoom = 1f;
            _origin = Vector2.Zero;
            _hasChanged = true;
        }

        private void UpdateMatrices()
        {
            var positionTranslationMatrix = Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0f));
            var rotationMatrix = Matrix.CreateRotationZ(_rotation);
            var scaleMatrix = Matrix.CreateScale(new Vector3(_zoom, _zoom, 1));
            var originTranslationMatrix = Matrix.CreateTranslation(new Vector3(_origin.X, _origin.Y, 0f));

            _transformationMatrix = positionTranslationMatrix * rotationMatrix * scaleMatrix * originTranslationMatrix;
            _inverseMatrix = Matrix.Invert(_transformationMatrix);
        }

        private void UpdateProjection()
        {
            Projection = Matrix.CreateOrthographic(Viewport.Width * _zoom, Viewport.Height * _zoom, 0f, -1f);
        }

        private void UpdateView()
        {
            var cameraPosition = new Vector3(_position, 0f);
            var cameraUpVector = Vector3.TransformNormal(Vector3.Up, Matrix.CreateRotationZ(_rotation));
            View = Matrix.CreateLookAt(cameraPosition, cameraPosition + Vector3.Forward, cameraUpVector);
        }
    }
}
