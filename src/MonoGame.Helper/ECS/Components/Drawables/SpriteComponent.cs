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
            Vector2 scale = default,
            bool fixedPosition = false) : base(spriteEffect, color, sourceRectangle, layerDepth, scale, fixedPosition)
        {
            Texture = texture;
        }

        public override Vector2 Origin => 0.5f * TextureSize;
        public Texture2D Texture { get; set; }
        public Vector2 TextureSize => Texture.Bounds.Size.ToVector2() * Scale;
    }
}
