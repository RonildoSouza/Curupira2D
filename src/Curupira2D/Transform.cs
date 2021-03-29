using Microsoft.Xna.Framework;

namespace Curupira2D
{
    public sealed class Transform
    {
        public Transform()
        {
            Position = Vector2.Zero;
            Rotation = 0f;
        }

        public Vector2 Position { get; private set; }
        public float Rotation { get; private set; }
        public float RotationInRadians => MathHelper.ToRadians(Rotation);

        public void SetPosition(float x, float y)
        {
            Position = new Vector2(x, y);
        }

        public void SetPosition(Vector2 position) => SetPosition(position.X, position.Y);

        public void SetRotation(float rotation)
        {
            Rotation = rotation;
        }

        public void SetTransform(Vector2 position, float rotation)
        {
            SetPosition(position);
            SetRotation(rotation);
        }
    }
}
