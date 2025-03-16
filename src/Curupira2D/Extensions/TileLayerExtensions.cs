using TiledLib.Layer;

namespace Curupira2D.Extensions
{
    public static class TileLayerExtensions
    {
        /// <summary>
        /// Get the global tile id (GID) from a layer
        /// </summary>
        /// <param name="layer">Current layer of iteration</param>
        /// <param name="x">Index of layer width iteration</param>
        /// <param name="y">Index of layer height iteration</param>
        public static uint GetGlobalTileId(this TileLayer layer, int x, int y) => layer?.Data[x + y * layer.Width] ?? 0;

        /// <summary>
        /// Check if a tile exists in a layer
        /// </summary>
        /// <param name="layer">Current layer of iteration</param>
        /// <param name="x">Index of layer width iteration</param>
        /// <param name="y">Index of layer height iteration</param>
        public static bool HasTile(this TileLayer layer, int x, int y) => GetGlobalTileId(layer, x, y) != 0;
    }
}
