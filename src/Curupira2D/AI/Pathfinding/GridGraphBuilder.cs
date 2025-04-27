using Curupira2D.AI.Pathfinding.Graphs;
using Curupira2D.ECS;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using System;
using TiledLib.Layer;

namespace Curupira2D.AI.Pathfinding
{
    public static class GridGraphBuilder
    {
        public static GridGraph Build(int width, int height, bool allowDiagonalSearch = false) => new(width, height, allowDiagonalSearch);

        /// <summary>
        /// Creates <see cref="GridGraph"/> from a <see cref="TileLayer"/>
        /// Present tile are walls and empty tiles are passable
        /// </summary>
        public static GridGraph Build(TileLayer tileLayer, bool allowDiagonalSearch = false)
        {
            ArgumentNullException.ThrowIfNull(tileLayer, nameof(tileLayer));

            var gridGraph = Build(tileLayer.Width, tileLayer.Height, allowDiagonalSearch);

            for (var y = 0; y < tileLayer.Height; y++)
                for (var x = 0; x < tileLayer.Width; x++)
                    if (tileLayer.HasTile(x, y))
                        gridGraph.Walls.Add(new System.Drawing.Point(x, y));

            return gridGraph;
        }

        public static System.Drawing.Point ToGridGraphPoint(this Vector2 position, int width, int height, Scene scene)
            => new((int)position.X / (scene.ScreenWidth / width), (int)scene.InvertPositionY(position.Y) / (scene.ScreenHeight / height));
    }
}
