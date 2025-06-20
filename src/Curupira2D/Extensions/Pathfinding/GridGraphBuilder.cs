using Curupira2D.AI.Pathfinding.Graphs;
using Curupira2D.ECS;
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

        public static System.Drawing.Point Vector2ToGridGraphPoint(this Vector2 vector2, Map map, Scene scene)
            => new((int)vector2.X / (scene.ScreenWidth / map.Width), (int)(vector2.Y / (scene.ScreenHeight / map.Height)));

        public static Vector2 GridGraphPointToPositionScene(this System.Drawing.Point point, Map map, Scene scene)
            => new((point.X * (scene.ScreenWidth / map.Width)) + (map.CellWidth * 0.5f), (point.Y * (scene.ScreenHeight / map.Height)) + (map.CellHeight * 0.5f));
    }
}
