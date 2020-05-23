using Microsoft.Xna.Framework;

namespace MonoGame.Helper.ECS.Components.Physics
{
    public class CollisionComponent : IComponent
    {
        public CollisionComponent(Point size)
        {
            Size = size;
        }

        public CollisionComponent(float width, float height) : this(new Point((int)width, (int)height)) { }

        public Point Location { get; private set; }
        public Point Size { get; private set; }
        public Rectangle BoundingBox => new Rectangle(Location, Size);

        public void SetLocation(int x, int y) => Location = new Point(x, y);

        public void SetLocation(float x, float y) => SetLocation((int)x, (int)y);

        public void SetLocation(Vector2 position) => SetLocation(position.X, position.Y);
    }
}
