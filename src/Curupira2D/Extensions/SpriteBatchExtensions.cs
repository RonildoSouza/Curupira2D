using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.Extensions
{
    public static class SpriteBatchExtensions
    {
        #region Draw Methods
        public static void Draw<TSpriteComponent>(this SpriteBatch spriteBatch, Vector2 position, float rotationInDegrees, TSpriteComponent spriteComponent) where TSpriteComponent : SpriteComponent
        {
            spriteBatch.Draw(
                spriteComponent.Texture,
                spriteComponent.Position ?? position,
                spriteComponent.SourceRectangle,
                spriteComponent.Color,
                MathHelper.ToRadians(rotationInDegrees),
                spriteComponent.Origin,
                spriteComponent.Scale,
                spriteComponent.SpriteEffect,
                spriteComponent.LayerDepth);
        }

        public static void Draw<TSpriteComponent>(this SpriteBatch spriteBatch, Vector2 position, TSpriteComponent spriteComponent) where TSpriteComponent : SpriteComponent
            => spriteBatch.Draw(position, 0f, spriteComponent);

        public static void Draw<TSpriteComponent>(this SpriteBatch spriteBatch, Entity entity, TSpriteComponent spriteComponent) where TSpriteComponent : SpriteComponent
            => spriteBatch.Draw(entity.Position, entity.Rotation, spriteComponent);
        #endregion

        #region DrawString Methods
        public static void DrawString(this SpriteBatch spriteBatch, Vector2 position, float rotationInDegrees, TextComponent textComponent)
        {
            spriteBatch.DrawString(
                textComponent.SpriteFont,
                textComponent.Text,
                textComponent.Position ?? position,
                textComponent.Color,
                MathHelper.ToRadians(rotationInDegrees),
                textComponent.Origin,
                textComponent.Scale,
                textComponent.SpriteEffect,
                textComponent.LayerDepth);
        }

        public static void DrawString(this SpriteBatch spriteBatch, Vector2 position, TextComponent textComponent)
            => spriteBatch.DrawString(position, 0f, textComponent);

        public static void DrawString(this SpriteBatch spriteBatch, Entity entity, TextComponent textComponent)
            => spriteBatch.DrawString(entity.Position, entity.Rotation, textComponent);
        #endregion
    }
}
