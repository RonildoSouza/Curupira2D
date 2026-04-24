using Curupira2D.AI.Pathfinding.Graphs;
using Curupira2D.Extensions.TiledMap;
using Microsoft.Xna.Framework;
using System;
using TiledLib;
using TiledLib.Layer;

namespace Curupira2D.Extensions.Pathfinding
{
    public static class GridGraphBuilder
    {
        public static GridGraph Build(int width, int height, bool allowDiagonalSearch = false) => new(width, height, allowDiagonalSearch);

        /// <summary>
        /// Creates <see cref="GridGraph"/> from a <see cref="TileLayer"/>
        /// Present tile are walls and empty tiles are passable
        /// </summary>
        /// <param name="tileLayer">The tiled map layer used as source.</param>
        /// <param name="allowDiagonalSearch">Whether diagonal movement is allowed.</param>
        /// <param name="scaleMultiplier">
        /// Multiplies the grid dimensions and subdivides each tile into (scaleMultiplier × scaleMultiplier)
        /// cells, increasing waypoint density and producing smoother A* paths.
        /// Must be >= 1. Default is 1 (no scaling).
        /// </param>
        public static GridGraph Build(TileLayer tileLayer, bool allowDiagonalSearch = false, int scaleMultiplier = 1)
        {
            if (scaleMultiplier < 1)
                throw new ArgumentException("scaleMultiplier must be >= 1", nameof(scaleMultiplier));

            int baseWidth = tileLayer.Width;
            int baseHeight = tileLayer.Height;

            int scaledWidth = baseWidth * scaleMultiplier;
            int scaledHeight = baseHeight * scaleMultiplier;

            var graph = new GridGraph(scaledWidth, scaledHeight, allowDiagonalSearch);

            for (int y = 0; y < baseHeight; y++)
            {
                for (int x = 0; x < baseWidth; x++)
                {
                    if (!tileLayer.HasTile(x, y))
                        continue;

                    // Each original tile becomes a (scaleMultiplier × scaleMultiplier) block of walls
                    int startX = x * scaleMultiplier;
                    int startY = y * scaleMultiplier;

                    for (int dy = 0; dy < scaleMultiplier; dy++)
                        for (int dx = 0; dx < scaleMultiplier; dx++)
                            graph.Walls.Add(new System.Drawing.Point(startX + dx, startY + dy));
                }
            }

            return graph;
        }

        /// <summary>
        /// Converts a 2D position to a grid graph point based on the specified map and scale multiplier.
        /// </summary>
        /// <remarks>This method divides each map cell into a grid of sub-cells according to the scale
        /// multiplier, allowing for finer granularity in grid-based calculations. The returned point reflects the
        /// sub-cell index within the overall grid.</remarks>
        /// <param name="position">The position in world coordinates to convert to a grid graph point.</param>
        /// <param name="map">The map that defines the cell dimensions used for the conversion. Cannot be null.</param>
        /// <param name="scaleMultiplier">The number of subdivisions per cell. Must be greater than zero 
        /// and the same value used when building the grid graph <see cref="Build(TileLayer, bool, int)"/>. Defaults to 1.</param>
        /// <returns>A System.Drawing.Point representing the grid graph coordinates corresponding to the specified position and
        /// scale.</returns>
        public static System.Drawing.Point Vector2ToGridGraphPoint(this Vector2 position, Map map, int scaleMultiplier = 1)
        {
            if (scaleMultiplier < 1)
                throw new ArgumentException("scaleMultiplier must be >= 1", nameof(scaleMultiplier));

            int tileW = map.CellWidth;
            int tileH = map.CellHeight;

            // Tile index no mapa original
            int tx = (int)(position.X / tileW);
            int ty = (int)(position.Y / tileH);

            // Offset sub-tile em pixels dentro do tile
            float subX = position.X - tx * tileW;
            float subY = position.Y - ty * tileH;

            // Tamanho de cada sub-célula em pixels
            float subCellW = (float)tileW / scaleMultiplier;
            float subCellH = (float)tileH / scaleMultiplier;

            int sx = (int)(subX / subCellW);
            int sy = (int)(subY / subCellH);

            return new System.Drawing.Point(
                tx * scaleMultiplier + Math.Clamp(sx, 0, scaleMultiplier - 1),
                ty * scaleMultiplier + Math.Clamp(sy, 0, scaleMultiplier - 1));
        }

        /// <summary>
        /// Converts a grid point in the map's logical grid to its corresponding position in scene coordinates.
        /// </summary>
        /// <remarks>Use this method to translate logical grid positions to scene coordinates for
        /// rendering or interaction purposes. The returned position corresponds to the center of the sub-cell defined
        /// by the scale multiplier.</remarks>
        /// <param name="gridPoint">The grid point to convert, specified in logical grid coordinates.</param>
        /// <param name="map">The map instance that defines the cell dimensions used for the conversion. Cannot be null.</param>
        /// <param name="scaleMultiplier">The number of subdivisions per cell. Must be greater than zero
        /// and the same value used when building the grid graph <see cref="Build(TileLayer, bool, int)"/>. Defaults to 1.</param>
        /// <returns>A Vector2 representing the center position of the specified grid point in scene coordinates.</returns>
        public static Vector2 GridGraphPointToPositionScene(this System.Drawing.Point gridPoint, Map map, int scaleMultiplier = 1)
        {
            if (scaleMultiplier < 1)
                throw new ArgumentException("scaleMultiplier must be >= 1", nameof(scaleMultiplier));

            float subCellW = (float)map.CellWidth / scaleMultiplier;
            float subCellH = (float)map.CellHeight / scaleMultiplier;

            return new Vector2(gridPoint.X * subCellW + subCellW * 0.5f, gridPoint.Y * subCellH + subCellH * 0.5f);
        }
    }
}
