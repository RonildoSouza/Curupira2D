using Microsoft.Xna.Framework;
using System;

namespace Curupira2D.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 GetSafeNormalize(this Vector2 vector2)
        {
            if (vector2 == Vector2.Zero)
                return Vector2.Zero;

            vector2.Normalize();
            return vector2;
        }

        public static Vector2 AngleToVector(this float angle) => new((float)Math.Sin(angle), (float)Math.Cos(angle));

        public static float VectorToAngle(this Vector2 vector) => (float)Math.Atan2(vector.X, vector.Y);

        public static Vector2 CartesianToIsometric(this Vector2 cartesianPosition)
                => new() { X = cartesianPosition.X - cartesianPosition.Y, Y = (cartesianPosition.X + cartesianPosition.Y) * 0.5f };

        public static Vector2 IsometricToCartesian(this Vector2 isometricPosition)
        {
            var cartesianPosition = Vector2.Zero;

            cartesianPosition.X = (isometricPosition.X + isometricPosition.Y * 2) / 2;
            cartesianPosition.Y = -isometricPosition.X + cartesianPosition.X;

            return cartesianPosition;
        }
    }
}
