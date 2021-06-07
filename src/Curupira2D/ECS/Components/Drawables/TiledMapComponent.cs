using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledLib;

namespace Curupira2D.ECS.Components.Drawables
{
    public class TiledMapComponent : SpriteComponent
    {
        public TiledMapComponent(
            Map map,
            Texture2D texture,
            Color color = default,
            bool drawWithoutUsingCamera = false) : base(texture: texture, color: color, drawWithoutUsingCamera: drawWithoutUsingCamera)
        {
            Map = map;
        }

        public override Vector2 Origin => new Vector2(Map.CellWidth * 0.5f, Map.CellHeight * 0.5f);
        public Map Map { get; }
    }
}
