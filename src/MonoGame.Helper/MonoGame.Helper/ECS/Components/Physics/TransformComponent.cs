using Microsoft.Xna.Framework;

namespace MonoGame.Helper.ECS.Components.Physics
{
    public class TransformComponent : IComponent
    {
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Radius { get; set; }
        public float Mass { get; set; }
        public float Gravity { get; set; }
    }
}
