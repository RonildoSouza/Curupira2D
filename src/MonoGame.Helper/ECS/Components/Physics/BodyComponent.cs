using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame.Helper.ECS.Components.Physics
{
    public class BodyComponent : IComponent
    {
        public BodyComponent(Vector2 size, float density = 1f)
        {
            Size = size;
            Force = Vector2.Zero;
            Density = density;
            EntityType = EntityType.Static;
            EntityShape = EntityShape.Rectangle;
            LinearVelocity = Vector2.Zero;
        }

        public BodyComponent(float width, float height, float density = 1f) : this(new Vector2(width, height), density) { }

        public Vector2 Size { get; }

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
        public float? Mass { get; set; }
        public float? Inertia { get; set; }
        public float Density { get; }
        /// <summary>
        /// Value between 0 and 1
        /// </summary>
        public float Restitution { get; set; }
        public float Friction { get; set; }
        public EntityType EntityType { get; set; }
        public EntityShape EntityShape { get; set; }
        public bool IgnoreGravity { get; set; }
        public IEnumerable<Vector2> Vertices { get; set; }
        public Vector2 LinearVelocity { get; set; }
        public bool FixedRotation { get; set; }
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
