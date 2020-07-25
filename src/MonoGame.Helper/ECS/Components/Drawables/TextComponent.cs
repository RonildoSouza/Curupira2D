using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Helper.ECS.Components.Drawables
{
    public class TextComponent : DrawableComponent
    {
        public TextComponent(
            SpriteFont spriteFont,
            string text,
            SpriteEffects spriteEffect = SpriteEffects.None,
            Color color = default,
            Rectangle? sourceRectangle = null,
            float layerDepth = 0,
            Vector2 scale = default) : base(spriteEffect, color, sourceRectangle, layerDepth, scale)
        {
            SpriteFont = spriteFont;
            Text = text;
        }

        public SpriteFont SpriteFont { get; set; }
        public string Text { get; set; }
        public override Vector2 Origin => 0.5f * SpriteFont.MeasureString(Text);
    }
}
