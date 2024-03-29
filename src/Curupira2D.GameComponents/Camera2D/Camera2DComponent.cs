﻿/*
 * https://manbeardgames.com/tutorials/2d-camera/
 * https://stackoverflow.com/questions/712296/xna-2d-camera-engine-that-follows-sprite
 * http://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Curupira2D.GameComponents.Camera2D
{
    public class Camera2DComponent : GameComponent, ICamera2D
    {
        private Matrix _transformationMatrix = Matrix.Identity;
        private Matrix _inverseMatrix = Matrix.Identity;
        private Vector2 _position = Vector2.Zero;
        private float _rotation = 0;
        private Vector2 _zoom = Vector2.One;
        private Vector2 _origin = Vector2.Zero;
        private bool _hasChanged;

        public Camera2DComponent(Game game, BasicEffect _spriteBatchEffect = null) : base(game)
        {
            Viewport = game.GraphicsDevice.Viewport;

            SpriteBatchEffect = _spriteBatchEffect ?? new BasicEffect(game.GraphicsDevice);
            SpriteBatchEffect.TextureEnabled = true;
            SpriteBatchEffect.VertexColorEnabled = true;
        }

        public Viewport Viewport { get; set; }
        public Matrix TransformationMatrix => _transformationMatrix;
        public Matrix InverseMatrix => _inverseMatrix;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                //  If the value hasn't actually changed, just return back
                if (_position == value) { return; }

                //  Set the position value
                _position = value;

                //  Flag that a change has been made
                _hasChanged = true;
            }
        }
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                //  If the value hasn't actually changed, just return back
                if (_rotation == value) { return; }

                //  Set the rotation value
                _rotation = value;

                //  Flag that a change has been made
                _hasChanged = true;
            }
        }
        public Vector2 Zoom
        {
            get { return _zoom; }
            set
            {
                //  If the value hasn't actually changed, just return back
                if (_zoom == value) { return; }

                if (value.X <= 0)
                    value.X = 1f;

                if (value.Y <= 0)
                    value.Y = 1f;

                //  Set the zoom value
                _zoom = value;

                //  Flag that a change has been made
                _hasChanged = true;
            }
        }
        public Vector2 Origin
        {
            get { return _origin; }
            set
            {
                //  If the value hasn't actually changed, just return back
                if (_origin == value) { return; }

                //  Set the origin value
                _origin = value;

                //  Flag that a change has been made
                _hasChanged = true;
            }
        }
        public float X
        {
            get { return _position.X; }
            set
            {
                //  If the value hasn't actually changed, just return back
                if (_position.X == value) { return; }

                //  Set the position x value
                _position.X = value;

                //  Flag that a change has been made
                _hasChanged = true;
            }
        }
        public float Y
        {
            get { return _position.Y; }
            set
            {
                //  If the value hasn't actually changed, just return back
                if (_position.Y == value) { return; }

                //  Set the position y value
                _position.Y = value;

                //  Flag that a change has been made
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

        public bool IsInView(Vector2 position, Texture2D texture)
        {
            // If the object is not within the horizontal bounds of the screen
            if (position.X + texture.Width < Position.X - Origin.X || position.X > Position.X + Origin.X)
                return false;

            // If the object is not within the vertical bounds of the screen
            if (position.Y + texture.Height < Position.Y - Origin.Y || position.Y > Position.Y + Origin.Y)
                return false;

            // In View
            return true;
        }

        public bool IsInView(float x, float y, Texture2D texture) => IsInView(new Vector2(x, y), texture);

        public void Reset()
        {
            _position = Vector2.Zero;
            _rotation = 0;
            _zoom = Vector2.One;
            _origin = Vector2.Zero;
        }

        /// <summary>
        /// Updates the values for our transformation matrix and the inverse matrix.  
        /// </summary>
        void UpdateMatrices()
        {
            //  Create a translation matrix based on the position of the camera
            var positionTranslationMatrix = Matrix.CreateTranslation(new Vector3
            {
                X = -(int)Math.Floor(_position.X),
                Y = -(int)Math.Floor(_position.Y),
                Z = 0
            });

            //  Create a rotation matrix around the Z axis
            var rotationMatrix = Matrix.CreateRotationZ(_rotation);

            //  Create a scale matrix based on the zoom
            var scaleMatrix = Matrix.CreateScale(new Vector3(_zoom.X, _zoom.Y, 1));

            //  Create a translation matrix based on the origin position of the camera
            var originTranslationMatrix = Matrix.CreateTranslation(new Vector3
            {
                X = (int)Math.Floor(_origin.X),
                Y = (int)Math.Floor(_origin.Y),
                Z = 0
            });

            //  Perform matrix multiplication of all of the above to create our transformation matrix
            _transformationMatrix = positionTranslationMatrix * rotationMatrix * scaleMatrix * originTranslationMatrix;

            //  Get our inverse matrix of the transformation matrix
            _inverseMatrix = Matrix.Invert(_transformationMatrix);
        }

        void UpdateProjection()
        {
            Projection = Matrix.CreateOrthographic(Viewport.Width * _zoom.X, Viewport.Height * _zoom.Y, 0f, -1f);
        }

        void UpdateView()
        {
            var cameraPosition = new Vector3(_position, 0f);
            var cameraUpVector = Vector3.TransformNormal(Vector3.Up, Matrix.CreateRotationZ(_rotation));
            View = Matrix.CreateLookAt(cameraPosition, cameraPosition + Vector3.Forward, cameraUpVector);
        }
    }
}
