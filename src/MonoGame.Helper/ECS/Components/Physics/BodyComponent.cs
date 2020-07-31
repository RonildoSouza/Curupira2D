using Microsoft.Xna.Framework;

namespace MonoGame.Helper.ECS.Components.Physics
{
    public class BodyComponent : IComponent
    {
        public BodyComponent()
        {
            Force = Vector2.Zero;
            Mass = 1f;
            Density = 1f;
            EntityType = EntityType.Static;
            EntityShape = EntityShape.Rectangle;
        }

        /// <summary>
        /// The world force vector, usually in Newtons (N).
        /// </summary>
        public Vector2 Force { get; set; }
        /// <summary>
        /// The world impulse vector, usually in N-seconds or kg-m/s.
        /// </summary>
        public Vector2 LinearImpulse { get; set; }
        /// <summary>
        /// The torque about the z-axis (out of the screen), usually in N-m.
        /// </summary>
        public float Torque { get; set; }
        /// <summary>
        /// The angular impulse in units of kg*m*m/s.
        /// </summary>
        public float AngularImpulse { get; set; }
        public float Radius { get; set; }
        public float Mass { get; set; }
        public float Inertia { get; set; }
        public float Density { get; set; }
        /// <summary>
        /// Value between 0 and 1
        /// </summary>
        public float Restitution { get; set; }
        public float Friction { get; set; }
        public EntityType EntityType { get; set; }
        public EntityShape EntityShape { get; set; }
        public bool IgnoreGravity { get; set; }
    }

    public enum EntityShape
    {
        Circle,
        Ellipse,
        Rectangle,
        Polygon,
    }

    public enum EntityType
    {
        /// <summary>
        /// Zero velocity, may be manually moved. Note: even static bodies have mass.
        /// </summary>
        Static,
        /// <summary>
        /// Zero mass, non-zero velocity set by user, moved by solver
        /// </summary>
        Kinematic,
        /// <summary>
        /// Positive mass, non-zero velocity determined by forces, moved by solver
        /// </summary>
        Dynamic,
    }
}
