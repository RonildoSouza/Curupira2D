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
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawInUICamera = true) : base(spriteEffect, color, sourceRectangle, layerDepth, scale, drawInUICamera)
        {
            SpriteFont = spriteFont;
            Text = text;
        }

        public override Vector2 Origin => TextSize * 0.5f;
        public SpriteFont SpriteFont { get; set; }
        public string Text { get; set; }
        public Vector2 TextSize => SpriteFont.MeasureString(Text);
    }
}
