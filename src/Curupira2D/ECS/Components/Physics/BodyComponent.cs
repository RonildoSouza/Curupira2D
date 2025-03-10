﻿using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;

namespace Curupira2D.ECS.Components.Physics
{
    public class BodyComponent : Body, IComponent
    {
        private float _radius;
        private float _restitution;

        public BodyComponent(Vector2 size, EntityType entityType, EntityShape entityShape, float density = 1f, Vector2 offset = default)
        {
            Size = size;
            EntityShape = entityShape;
            BodyType = (BodyType)entityType;
            Density = density;
            Offset = offset;
        }

        public BodyComponent(float width, float height, EntityType entityType, EntityShape entityShape, float density = 1f, Vector2 offset = default)
            : this(new Vector2(width, height), entityType, entityShape, density, offset) { }

        /// <summary>
        /// Create circle shape body
        /// </summary>
        public BodyComponent(float radius, EntityType entityType, float density = 1f, Vector2 offset = default)
            : this(Vector2.Zero, entityType, EntityShape.Circle, density, offset)
        {
            Radius = radius;
            Offset = offset;
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

        /// <summary>
        /// Set offset to Circle and Rectangle
        /// </summary>
        public Vector2 Offset { get; }

        public Vector2 SetLinearVelocityX(float x)
            => LinearVelocity = new Vector2(x, LinearVelocity.Y);

        public Vector2 SetLinearVelocityY(float y)
            => LinearVelocity = new Vector2(LinearVelocity.X, y);

        public void ApplyLinearImpulseX(float x)
            => ApplyLinearImpulse(new Vector2(x, 0f));

        public void ApplyLinearImpulseY(float y)
            => ApplyLinearImpulse(new Vector2(0f, y));

        public void ApplyLinearImpulse(float x, float y)
            => ApplyLinearImpulse(new Vector2(x, y));

        public void ApplyForce(float x, float y)
            => ApplyForce(new Vector2(x, y));

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
        PolyLine,
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
