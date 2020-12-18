using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Helper
{
    public class Camera : Comora.Camera
    {
        readonly GraphicsDevice _graphicsDevice;

        public Camera(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateProjection();
            UpdateView();
        }

        void UpdateProjection()
        {
            Projection = Matrix.CreateOrthographic(_graphicsDevice.Viewport.Width * Zoom, _graphicsDevice.Viewport.Height * Zoom, 0f, 30f);
        }

        void UpdateView()
        {
            var cameraPosition = new Vector3(Position, 0f);
            var cameraUpVector = Vector3.TransformNormal(Vector3.Down, Matrix.CreateRotationZ(Rotation));
            View = Matrix.CreateLookAt(cameraPosition, cameraPosition + Vector3.Backward, cameraUpVector);
        }
    }
}
