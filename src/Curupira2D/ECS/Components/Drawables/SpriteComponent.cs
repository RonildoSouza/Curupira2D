using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Curupira2D.ECS.Components.Drawables
{
    public class SpriteComponent : DrawableComponent
    {
        protected Texture2D _texture;
        protected Color[] _textureData;

        public SpriteComponent(
            Texture2D texture,
            SpriteEffects spriteEffect = SpriteEffects.FlipVertically,
            Color color = default,
            Rectangle? sourceRectangle = null,
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawInUICamera = false) : base(spriteEffect, color, sourceRectangle, layerDepth, scale, drawInUICamera)
        {
            Texture = texture;
        }

        public virtual Texture2D Texture
        {
            get => _texture;
            set
            {
                _texture = value;
                Origin = SourceRectangle.HasValue ? SourceRectangle.Value.Size.ToVector2() * 0.5f : TextureSize * 0.5f;
            }
        }
        public Vector2 TextureSize => Texture.Bounds.Size.ToVector2();

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
