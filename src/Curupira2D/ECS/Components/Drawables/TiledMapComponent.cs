using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledLib;
using System;

namespace Curupira2D.ECS.Components.Drawables
{
    public class TiledMapComponent : SpriteComponent
    {
        public TiledMapComponent(
            Map map,
            Texture2D texture,
            Color color = default,
            bool drawInUICamera = false) : base(texture: texture, color: color, drawInUICamera: drawInUICamera)
        {
            Map = map;
            Origin = new Vector2(Map.CellWidth * 0.5f, Map.CellHeight * 0.5f);
        }

        public override float LayerDepth => throw new NotImplementedException("Layer depth can't be define!");
        public Map Map { get; }
    }
}
