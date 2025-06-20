using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.ECS.Components.Drawables
{
    public sealed class SpriteComponent : DrawableComponent
    {
        private Texture2D _texture;
        private Color[] _textureData;

        public SpriteComponent(
            Texture2D texture,
            SpriteEffects spriteEffect = SpriteEffects.None,
            Color color = default,
            Rectangle? sourceRectangle = null,
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawInUICamera = false) : base(texture, spriteEffect, color, sourceRectangle, layerDepth, scale, drawInUICamera)
        {
            Origin = SourceRectangle.HasValue ? SourceRectangle.Value.Size.ToVector2() * 0.5f : TextureSize * 0.5f;
        }

        public override Texture2D Texture
        {
            get => _texture;
            set
            {
                _texture = value;
                Origin = SourceRectangle.HasValue ? SourceRectangle.Value.Size.ToVector2() * 0.5f : TextureSize * 0.5f;
            }
        }

        public override Color[] TextureData
        {
            get
            {
                if (_texture is null)
                    return [];

                _textureData = new Color[(int)(TextureSize.X * TextureSize.Y)];
                _texture.GetData(_textureData);

                return _textureData;
            }
        }
    }
}
