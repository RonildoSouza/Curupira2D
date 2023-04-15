using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;

namespace Curupira2D.ECS.Components.Drawables
{
    public class TextComponent : DrawableComponent
    {
        string _text;

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
            Origin = Half = TextSize * 0.5f;
        }

        public SpriteFont SpriteFont { get; set; }
        public string Text
        {
            get => _text;
            set
            {
                _text = value;

                //if (TextSize != Vector2.Zero)
                //{
                //    TextureData = new Color[SpriteFont.Texture.Width * SpriteFont.Texture.Height / 4];
                //    SpriteFont.Texture.GetData(TextureData);
                //}
            }
        }
        public Vector2 TextSize => SpriteFont.MeasureString(Text);
    }
}
