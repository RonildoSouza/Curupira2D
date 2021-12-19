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

        public static Vector2 AngleToVector(this float angle)
             => new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));

        public static float VectorToAngle(this Vector2 vector)
             => (float)Math.Atan2(vector.X, -vector.Y);
    }
}
