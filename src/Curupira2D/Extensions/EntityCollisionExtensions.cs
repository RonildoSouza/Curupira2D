using Curupira2D.ECS;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Curupira2D.Extensions
{
    public static class EntityCollisionExtensions
    {
        public static bool IsCollidedWithAny(this Entity entity, Scene scene, bool perPixelCollision = false)
            => IsCollidedWith(entity, scene, _ => perPixelCollision ? entity.PerPixelCollision(_) : entity.GetHitBox().Intersects(_.GetHitBox()));

        public static bool IsCollidedWithAny(this Entity entity, Scene scene, string entityGroup, bool perPixelCollision = false)
            => IsCollidedWith(entity, scene, _ => _.Group == entityGroup && (perPixelCollision ? entity.PerPixelCollision(_) : entity.GetHitBox().Intersects(_.GetHitBox())));

        public static bool IsCollidedWith(this Entity entity, Scene scene, Entity otherEntity, bool perPixelCollision = false)
            => IsCollidedWith(entity, scene, _ => _.Equals(otherEntity) && (perPixelCollision ? entity.PerPixelCollision(_) : entity.GetHitBox().Intersects(_.GetHitBox())));

        public static bool IsCollidedWith(this Entity entity, Scene scene, string otherEntityUniqueId, bool perPixelCollision = false)
            => IsCollidedWith(entity, scene, _ => _.UniqueId == otherEntityUniqueId && (perPixelCollision ? entity.PerPixelCollision(_) : entity.GetHitBox().Intersects(_.GetHitBox())));

        /// <summary>
        /// https://gamedev.stackexchange.com/questions/15191/is-there-a-good-way-to-get-pixel-perfect-collision-detection-in-xna
        /// </summary>
        static bool PerPixelCollision(this Entity entity, Entity otherEntity)
        {
            var transformA = CreateTranslation(entity);
            var dataA = entity.GetDrawableComponent().TextureData;
            var widthA = entity.GetHitBox().Width;
            var heightA = entity.GetHitBox().Height;

            var transformB = CreateTranslation(otherEntity);
            var dataB = otherEntity.GetDrawableComponent().TextureData;
            var widthB = otherEntity.GetHitBox().Width;
            var heightB = otherEntity.GetHitBox().Height;

            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)posInB.X;
                    int yB = (int)posInB.Y;

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = (xA + yA * widthA) + 1 > dataA.Length
                            ? dataA[xA + yA * (widthA / 4)]
                            : dataA[xA + yA * widthA];
                        Color colorB = (xB + yB * widthB) + 1 > dataB.Length
                            ? dataB[xB + yB * (widthB / 4)]
                            : dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        // then an intersection has been found
                        if (colorA.A != 0 && colorB.A != 0)
                            return true;
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;

            static Matrix CreateTranslation(Entity entity)
            {
                var drawableComponent = entity.GetDrawableComponent();

                return Matrix.CreateTranslation(
                        new Vector3(-(drawableComponent?.Origin ?? Vector2.Zero), 0.0f)) *
                        Matrix.CreateRotationZ(entity.Rotation) *
                        Matrix.CreateTranslation(new Vector3(entity.Position, 0.0f));
            }
        }

        static bool IsCollidedWith(Entity entity, Scene scene, Func<Entity, bool> predicate)
        {
            if (!entity.Active || !entity.IsCollidable)
                return false;

            scene.Quadtree.Delete(entity);
            scene.Quadtree.Insert(entity);

            var returnObjects = scene.Quadtree.Retrieve(entity);
            return returnObjects.Any(predicate);
        }
    }
}
