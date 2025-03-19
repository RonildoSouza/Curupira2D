using Curupira2D.AI.Pathfinding.Graphs;
using Curupira2D.Extensions;
using System;
using System.Drawing;
using TiledLib.Layer;

namespace Curupira2D.AI.Pathfinding
{
    public static class GridGraphBuilder
    {
        public static GridGraph Build(int width, int height, bool allowDiagonalSearch = false) => new(width, height, allowDiagonalSearch);

        public static GridGraph Build(TileLayer tileLayer, bool allowDiagonalSearch = false)
        {
            ArgumentNullException.ThrowIfNull(tileLayer, nameof(tileLayer));

            var gridGraph = Build(tileLayer.Width, tileLayer.Height, allowDiagonalSearch);

            for (var y = 0; y < tileLayer.Height; y++)
                for (var x = 0; x < tileLayer.Width; x++)
                    if (tileLayer.HasTile(x, y))
                        gridGraph.Walls.Add(new Point(x, y));


            return gridGraph;
        }
    }
}
