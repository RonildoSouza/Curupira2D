using Microsoft.Xna.Framework;

namespace MonoGame.Helper.ECS
{
    public sealed class Transform
    {
        public Transform()
        {
            Position = Vector2.Zero;
            RotationInDegrees = 0f;
        }

        public Vector2 Position { get; private set; }
        public float RotationInDegrees { get; private set; }
        public float RotationInRadians => MathHelper.ToRadians(RotationInDegrees);

        public void SetPosition(float x, float y)
        {
            Position = new Vector2(x, y);
        }

        public void SetPosition(Vector2 position) => SetPosition(position.X, position.Y);

        public void SetRotation(float rotationInDegrees)
        {
            RotationInDegrees = rotationInDegrees;
        }

        public void SetTransform(Vector2 position, float rotationInDegrees)
        {
            SetPosition(position);
            SetRotation(rotationInDegrees);
        }
    }
}
