using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.ECS.Components.Drawables
{
    public class SpriteComponent : DrawableComponent
    {
        public SpriteComponent(
            Texture2D texture,
            SpriteEffects spriteEffect = SpriteEffects.FlipVertically,
            Color color = default,
            Rectangle? sourceRectangle = null,
            float layerDepth = 0,
            Vector2 scale = default,
            bool drawWithoutUsingCamera = true) : base(spriteEffect, color, sourceRectangle, layerDepth, scale, drawWithoutUsingCamera)
        {
            Texture = texture;
        }

        public override Vector2 Origin => SourceRectangle.HasValue ? SourceRectangle.Value.Size.ToVector2() * 0.5f : 0.5f * TextureSize;
        public Texture2D Texture { get; set; }
        public Vector2 TextureSize => Texture.Bounds.Size.ToVector2();
    }
}
