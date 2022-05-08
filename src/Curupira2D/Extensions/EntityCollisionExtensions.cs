using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Curupira2D.Extensions
{
    public static class EntityCollisionExtensions
    {
        public static bool IsCollidedWithAny(this Entity entity, Scene scene)
            => IsCollidedWith(entity, scene, _ => entity.PerPixelCollision(_));

        public static bool IsCollidedWithAny(this Entity entity, Scene scene, string entityGroup)
            => IsCollidedWith(entity, scene, _ => _.Group == entityGroup && entity.PerPixelCollision(_));

        public static bool IsCollidedWith(this Entity entity, Scene scene, Entity otherEntity)
            => IsCollidedWith(entity, scene, _ => _.Equals(otherEntity) && entity.PerPixelCollision(_));

        public static bool IsCollidedWith(this Entity entity, Scene scene, string otherEntityUniqueId)
            => IsCollidedWith(entity, scene, _ => _.UniqueId == otherEntityUniqueId && entity.PerPixelCollision(_));

        /// <summary>
        /// https://gamedev.stackexchange.com/questions/15191/is-there-a-good-way-to-get-pixel-perfect-collision-detection-in-xna
        /// </summary>
        static bool PerPixelCollision(this Entity entity, Entity otherEntity)
        {
            var transformA = CreateTranslation(entity);
            var dataA = GetTextureData(entity);
            var widthA = entity.GetHitBox().Width;
            var heightA = entity.GetHitBox().Height;

            var transformB = CreateTranslation(otherEntity);
            var dataB = GetTextureData(otherEntity);
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
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

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

        static Color[] GetTextureData(Entity entity)
        {
            if (entity.Components.Any(_ => _.Key == typeof(SpriteComponent) || _.Key == typeof(SpriteAnimationComponent)))
            {
                Color[] textureData = null;
                var spriteComponent = entity.GetComponent<SpriteComponent>() ?? entity.GetComponent<SpriteAnimationComponent>();

                if (spriteComponent is SpriteComponent)
                {
                    textureData = new Color[spriteComponent.Texture.Width * spriteComponent.Texture.Height];
                    spriteComponent.Texture.GetData(textureData);
                }

                if (spriteComponent is SpriteAnimationComponent)
                {
                    textureData = new Color[spriteComponent.SourceRectangle.Value.Width * spriteComponent.SourceRectangle.Value.Height];
                    spriteComponent.Texture.GetData(0, spriteComponent.SourceRectangle, textureData, 0, spriteComponent.SourceRectangle.Value.Width * spriteComponent.SourceRectangle.Value.Height);
                }

                return textureData;
            }

            if (entity.Components.Any(_ => _.Key == typeof(TextComponent)))
            {
                var textComponent = entity.GetComponent<TextComponent>();
                var textureData = new Color[textComponent.SpriteFont.Texture.Width * textComponent.SpriteFont.Texture.Height / 4];
                textComponent.SpriteFont.Texture.GetData(textureData);

                return textureData;
            }

            return null;
        }
    }
}
