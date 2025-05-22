using System;
using System.Collections.Generic;
using System.Linq;
using TiledLib;
using TiledLib.Layer;

namespace Curupira2D.Extensions.TiledMap
{
    public static class BaseLayerExtensions
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
        public static bool HasTile(this TileLayer layer, int x, int y) => layer.GetGlobalTileId(x, y) != 0;

        public static T Get<T>(this Map map, string name) where T : BaseLayer
            => map.GetAll<T>().FirstOrDefault(_ => _.Name == name);

        public static T Get<T>(this Map map, int id) where T : BaseLayer
            => map.GetAll<T>().FirstOrDefault(_ => _.Id == id);

        public static IEnumerable<T> GetAll<T>(this Map map) where T : BaseLayer
            => map.Layers.OfType<T>();

        public static IEnumerable<T> GetAll<T>(this Map map, Func<T, bool> predicate) where T : BaseLayer
            => map.Layers.OfType<T>().Where(predicate);
    }
}
