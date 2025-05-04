using Curupira2D.AI.Pathfinding.AStar;
using System.Drawing;

namespace Curupira2D.AI.Pathfinding.Graphs
{
    public class EdgesPointGraph : EdgesGraph<Point>, IAStarGraph<Point>
    {
        public new EdgesPointGraph AddEdgesForNode(Point node, Point[] edges)
            => (EdgesPointGraph)base.AddEdgesForNode(node, edges);

        public new EdgesPointGraph AddWeights(Point fromNode, Point toNode, int weights)
             => (EdgesPointGraph)base.AddWeights(fromNode, toNode, weights);

        public int Heuristic(Point node, Point goal) => Math.Abs(node.X - goal.X) + Math.Abs(node.Y - goal.Y);
    }
}
