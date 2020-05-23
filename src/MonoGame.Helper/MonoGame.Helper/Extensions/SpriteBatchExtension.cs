using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS.Components.Drawables;

namespace MonoGame.Helper.Extensions
{
    public static class SpriteBatchExtension
    {
        public static void Draw(this SpriteBatch spriteBatch, Vector2 position, float rotationInDegrees, SpriteComponent spriteComponent)
        {
            spriteBatch.Draw(
                spriteComponent.Texture,
                position,
                spriteComponent.SourceRectangle,
                spriteComponent.Color,
                MathHelper.ToRadians(rotationInDegrees),
                Vector2.Zero,
                spriteComponent.Scale,
                spriteComponent.SpriteEffect,
                spriteComponent.LayerDepth);
        }

        public static void Draw(this SpriteBatch spriteBatch, Vector2 position, SpriteComponent spriteComponent)
            => Draw(spriteBatch, position, 0f, spriteComponent);

        public static void DrawString(this SpriteBatch spriteBatch, Vector2 position, float rotationInDegrees, TextComponent textComponent)
        {
            spriteBatch.DrawString(
                textComponent.SpriteFont,
                textComponent.Text,
                position,
                textComponent.Color,
                MathHelper.ToRadians(rotationInDegrees),
                Vector2.Zero,
                textComponent.Scale,
                textComponent.SpriteEffect,
                textComponent.LayerDepth);
        }

        public static void DrawString(this SpriteBatch spriteBatch, Vector2 position, TextComponent textComponent)
            => DrawString(spriteBatch, position, 0f, textComponent);
    }
}
