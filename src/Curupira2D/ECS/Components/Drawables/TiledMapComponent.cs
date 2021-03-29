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
            bool fixedPosition = false) : base(texture, SpriteEffects.None, color, null, 0, default, fixedPosition)
        {
            Map = map;
        }

        public override Vector2 Origin => new Vector2(Map.CellWidth * 0.5f, Map.CellHeight * 0.5f);
        public Map Map { get; }
    }
}
