using Curupira2D.ECS.Components.Drawables;
using System.Linq;
using TiledLib.Layer;

namespace Curupira2D.Extensions
{
    public static class TiledMapComponentExtensions
    {
        public static TileLayer GetTileLayer(this TiledMapComponent tiledMapComponent, string name)
            => tiledMapComponent.Map.Layers.OfType<TileLayer>().Get(name);

        public static TileLayer GetTileLayer(this TiledMapComponent tiledMapComponent, int id)
            => tiledMapComponent.Map.Layers.OfType<TileLayer>().Get(id);
    }
}
