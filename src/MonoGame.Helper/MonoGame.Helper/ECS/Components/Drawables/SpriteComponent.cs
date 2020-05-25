using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Helper.ECS.Components.Drawables
{
    public class SpriteComponent : DrawableComponent
    {
        public SpriteComponent(
            Texture2D texture,
            SpriteEffects spriteEffect = SpriteEffects.None,
            Color color = default,
            Rectangle? sourceRectangle = null,
            float layerDepth = 0,
            Vector2 scale = default) : base(spriteEffect, color, sourceRectangle, layerDepth, scale)
        {
            Texture = texture;
        }

        public Texture2D Texture { get; set; }
        public Vector2 TextureSize => Texture.Bounds.Size.ToVector2() * Scale;
        //public Vector2 TextureOrigin => 0.5f * TextureSize;
    }
}
