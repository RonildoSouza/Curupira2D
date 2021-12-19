using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Dynamics;

namespace Curupira2D.ECS.Components.Physics
{
    public class BodyComponent : Body, IComponent
    {
        private float _radius;
        private float _restitution;

        public BodyComponent(Vector2 size, EntityType entityType, EntityShape entityShape, float density = 1f)
        {
            Size = size;
            EntityShape = entityShape;
            BodyType = (BodyType)entityType;
            Density = density;
        }

        public BodyComponent(float width, float height, EntityType entityType, EntityShape entityShape, float density = 1f)
            : this(new Vector2(width, height), entityType, entityShape, density) { }

        /// <summary>
        /// Create circle shape body
        /// </summary>
        public BodyComponent(float radius, EntityType entityType, float density = 1f)
            : this(Vector2.Zero, entityType, EntityShape.Circle, density)
        {
            Radius = radius;
        }

        public BodyComponent(IEnumerable<Vector2> vertices, EntityType entityType, EntityShape entityShape, float density = 1f)
            : this(Vector2.Zero, entityType, entityShape, density)
        {
            Vertices = vertices;
        }

        public Vector2 Size { get; }

        public EntityShape EntityShape { get; }

        public float Radius
        {
            get
            {
                ValidateRadiusValue(_radius);
                return _radius;
            }
            set
            {
                ValidateRadiusValue(value);
                _radius = value;
            }
        }

        public float Density { get; }

        public float Restitution
        {
            get => _restitution;
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("The restitution value must be between 0 and 1!");

                _restitution = value;
            }
        }

        public float Friction { get; set; }

        public IEnumerable<Vector2> Vertices { get; set; }

        public Vector2 SetLinearVelocityX(float x)
            => LinearVelocity = new Vector2(x, LinearVelocity.Y);

        public Vector2 SetLinearVelocityY(float y)
            => LinearVelocity = new Vector2(LinearVelocity.X, y);

        public void ApplyLinearImpulseX(float x)
            => ApplyLinearImpulse(new Vector2(x, 0f));

        public void ApplyLinearImpulseY(float y)
            => ApplyLinearImpulse(new Vector2(0f, y));

        public Vector2 RotationToVector() => Rotation.AngleToVector();

        void ValidateRadiusValue(float radius)
        {
            if (EntityShape == EntityShape.Circle && radius == 0)
                throw new ArgumentException($"Radius value can't be 0 when {nameof(EntityShape)} equals {nameof(EntityShape.Circle)}!");
        }
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
