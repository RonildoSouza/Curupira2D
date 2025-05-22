using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TiledLib;

namespace Curupira2D.ECS.Components.Drawables
{
    public sealed class TiledMapComponent : DrawableComponent
    {
        private Texture2D _texture;
        private Color[] _textureData;

        public TiledMapComponent(Map map, Texture2D texture, Color color = default, bool drawInUICamera = false)
            : base(texture, color: color, drawInUICamera: drawInUICamera)
        {
            Map = map;

            if (Map != null)
                Origin = new Vector2(Map.CellWidth * 0.5f, Map.CellHeight * 0.5f);
        }

        public override float LayerDepth => throw new NotImplementedException("Layer depth can't be define!");

        public override Texture2D Texture
        {
            get => _texture;
            set => _texture = value;
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

        public Map Map { get; }
    }
}
