using Microsoft.Xna.Framework;

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
    }
}
