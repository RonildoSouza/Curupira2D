using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.ECS.Components.Drawables
{
    public class TextComponent : DrawableComponent
    {
        public TextComponent(
            SpriteFont spriteFont,
            string text,
            SpriteEffects spriteEffect = SpriteEffects.FlipVertically,
            Color color = default,
            Rectangle? sourceRectangle = null,
            float layerDepth = 0,
            Vector2 scale = default,
            bool drawWithoutUsingCamera = true) : base(spriteEffect, color, sourceRectangle, layerDepth, scale, drawWithoutUsingCamera)
        {
            SpriteFont = spriteFont;
            Text = text;
        }

        public override Vector2 Origin => 0.5f * SpriteFont.MeasureString(Text);
        public SpriteFont SpriteFont { get; set; }
        public string Text { get; set; }
    }
}
