/*
 * https://manbeardgames.com/tutorials/2d-camera/
 * https://stackoverflow.com/questions/712296/xna-2d-camera-engine-that-follows-sprite
 * http://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Helper.GameComponents.Camera2D
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

        public Camera2DComponent(Game game) : base(game) { }

        public Viewport Viewport { get; set; }
        public Matrix TransformationMatrix
        {
            get
            {
                //  If a change is detected, update matrices before
                //  returning value
                if (_hasChanged)
                {
                    UpdateMatrices();
                    UpdateProjection();
                    UpdateView();
                }
                return _transformationMatrix;
            }
        }
        public Matrix InverseMatrix
        {
            get
            {
                //  If a change is detected, update matrices before
                //  returning value
                if (_hasChanged)
                {
                    UpdateMatrices();
                    UpdateProjection();
                    UpdateView();
                }
                return _inverseMatrix;
            }
        }
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

        public override void Initialize()
        {
            Viewport = Game.GraphicsDevice.Viewport;
            base.Initialize();
        }

        public Vector2 ScreenToWorld(Vector2 position)
        {
            return Vector2.Transform(position, InverseMatrix);
        }

        public Vector2 WorldToScreen(Vector2 position)
        {
            return Vector2.Transform(position, TransformationMatrix);
        }

        public bool IsInView(Vector2 position, Texture2D texture)
        {
            // If the object is not within the horizontal bounds of the screen
            if ((position.X + texture.Width) < (Position.X - Origin.X) || (position.X) > (Position.X + Origin.X))
                return false;

            // If the object is not within the vertical bounds of the screen
            if ((position.Y + texture.Height) < (Position.Y - Origin.Y) || (position.Y) > (Position.Y + Origin.Y))
                return false;

            // In View
            return true;
        }

        /// <summary>
        ///     Updates the values for our transformation matrix and 
        ///     the inverse matrix.  
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

            //  Since the matrices have now been updated, set that there is no longer a change
            _hasChanged = false;
        }

        void UpdateProjection()
        {
            Projection = Matrix.CreateOrthographic(Viewport.Width * Zoom.X, Viewport.Height * Zoom.Y, 0f, 30f);
        }

        void UpdateView()
        {
            var cameraPosition = new Vector3(Position, 0f);
            var cameraUpVector = Vector3.TransformNormal(Vector3.Down, Matrix.CreateRotationZ(Rotation));
            View = Matrix.CreateLookAt(cameraPosition, cameraPosition + Vector3.Backward, cameraUpVector);
        }
    }
}
