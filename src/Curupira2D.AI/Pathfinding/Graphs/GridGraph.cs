using Curupira2D.AI.Pathfinding.AStar;
using Curupira2D.AI.Pathfinding.BreadthFirst;
using Curupira2D.AI.Pathfinding.Dijkstra;
using System.Drawing;

namespace Curupira2D.AI.Pathfinding.Graphs
{
    public class GridGraph(int width, int height, bool allowDiagonalSearch = false) : IUnweightedGraph<Point>, IWeightedGraph<Point>, IAStarGraph<Point>
    {
        protected readonly List<Point> _neighbors = new(4);

        /// <summary>
        /// East, North, West, South
        /// </summary>,
        protected static Point[] CardinalDirections => [
            new(1, 0), new(0, -1), new(-1, 0), new(0, 1),
        ];

        /// <summary>
        /// East, North-East, North, North-West, West, South-West, South, South-East
        /// </summary>,
        protected static Point[] CompassDirections => [
            new(1, 0), new(1, -1), new(0, -1), new(-1, -1),
            new(-1, 0), new(-1, 1), new(0, 1), new(1, 1),
        ];

        protected virtual Point[] Directions => allowDiagonalSearch ? CompassDirections : CardinalDirections;

        public int Width => width;
        public int Height => height;
        public HashSet<Point> Walls { get; set; } = [];
        public int DefaultWeight { get; set; } = 1;
        public int Weights { get; set; } = 5;
        public HashSet<Point> WeightedNodes { get; set; } = [];

        public bool IsInBounds(Point node) => 0 <= node.X && node.X < width && 0 <= node.Y && node.Y < height;

        public bool IsPassable(Point node) => !Walls.Contains(node);

        public IEnumerable<Point> GetNeighbors(Point node)
        {
            _neighbors.Clear();

            foreach (var dir in Directions)
            {
                var next = new Point(node.X + dir.X, node.Y + dir.Y);

                if (IsInBounds(next) && IsPassable(next))
                    _neighbors.Add(next);
            }

            // Fixes ugly paths
            if ((node.X + node.Y) % 2 == 0)
                _neighbors.Reverse();

            return _neighbors;
        }

        public int Cost(Point fromNode, Point toNode) => WeightedNodes.Contains(toNode) ? Weights : DefaultWeight;

        public int Heuristic(Point node, Point goal) => Math.Abs(node.X - goal.X) + Math.Abs(node.Y - goal.Y);
    }
}