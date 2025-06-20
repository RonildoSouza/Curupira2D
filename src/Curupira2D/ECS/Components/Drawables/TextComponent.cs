using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.ECS.Components.Drawables
{
    public sealed class TextComponent : DrawableComponent
    {
        string _text;
        Color[] _textureData;

        public TextComponent(
            SpriteFont spriteFont,
            string text,
            SpriteEffects spriteEffect = SpriteEffects.None,
            Color color = default,
            Rectangle? sourceRectangle = null,
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawInUICamera = true) : base(null, spriteEffect, color, sourceRectangle, layerDepth, scale, drawInUICamera)
        {
            SpriteFont = spriteFont;
            Text = text;
            Origin = TextSize * 0.5f;
        }

        public SpriteFont SpriteFont { get; set; }
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                Origin = TextSize * 0.5f;
            }
        }

        public Vector2 TextSize => SpriteFont.MeasureString(Text); public override Color[] TextureData
        {
            get
            {
                if (SpriteFont.Texture is null)
                    return [];

                if (TextSize != Vector2.Zero)
                {
                    _textureData = new Color[SpriteFont.Texture.Width * SpriteFont.Texture.Height / 4];
                    SpriteFont.Texture.GetData(_textureData);
                }

                return _textureData;
            }
        }
    }
}
