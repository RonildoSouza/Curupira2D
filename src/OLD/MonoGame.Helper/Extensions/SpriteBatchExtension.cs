using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;

namespace MonoGame.Helper.Extensions
{
    public static class SpriteBatchExtension
    {
        public static void Draw<TSpriteComponent>(this SpriteBatch spriteBatch, Vector2 position, float rotationInDegrees, TSpriteComponent spriteComponent) where TSpriteComponent : SpriteComponent
        {
            spriteBatch.Draw(
                spriteComponent.Texture,
                position,
                spriteComponent.SourceRectangle,
                spriteComponent.Color,
                MathHelper.ToRadians(rotationInDegrees),
                spriteComponent.Origin,
                spriteComponent.Scale,
                spriteComponent.SpriteEffect,
                spriteComponent.LayerDepth);
        }

        public static void Draw<TSpriteComponent>(this SpriteBatch spriteBatch, Vector2 position, TSpriteComponent spriteComponent) where TSpriteComponent : SpriteComponent
            => Draw(spriteBatch, position, 0f, spriteComponent);

        public static void Draw<TSpriteComponent>(this SpriteBatch spriteBatch, Entity entity, TSpriteComponent spriteComponent) where TSpriteComponent : SpriteComponent
            => Draw(spriteBatch, entity.Transform.Position, entity.Transform.Rotation, spriteComponent);

        public static void DrawString(this SpriteBatch spriteBatch, Vector2 position, float rotationInDegrees, TextComponent textComponent)
        {
            spriteBatch.DrawString(
                textComponent.SpriteFont,
                textComponent.Text,
                position,
                textComponent.Color,
                MathHelper.ToRadians(rotationInDegrees),
                textComponent.Origin,
                textComponent.Scale,
                textComponent.SpriteEffect,
                textComponent.LayerDepth);
        }

        public static void DrawString(this SpriteBatch spriteBatch, Vector2 position, TextComponent textComponent)
            => DrawString(spriteBatch, position, 0f, textComponent);

        public static void DrawString(this SpriteBatch spriteBatch, Entity entity, TextComponent textComponent)
            => DrawString(spriteBatch, entity.Transform.Position, entity.Transform.Rotation, textComponent);
    }
}
