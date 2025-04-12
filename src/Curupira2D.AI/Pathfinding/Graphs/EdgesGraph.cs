using Curupira2D.AI.Pathfinding.BreadthFirst;
using Curupira2D.AI.Pathfinding.Dijkstra;

namespace Curupira2D.AI.Pathfinding.Graphs
{
    public class EdgesGraph<T> : IUnweightedGraph<T>, IWeightedGraph<T> where T : notnull
    {
        public Dictionary<T, T[]> Edges { get; internal set; } = [];
        public Dictionary<(T, T), int> WeightedNodes { get; internal set; } = [];
        public int DefaultWeight { get; set; } = 1;

        public IEnumerable<T> GetNeighbors(T node) => Edges[node];

        public int Cost(T fromNode, T toNode)
            => WeightedNodes.ContainsKey((fromNode, toNode)) ? WeightedNodes[(fromNode, toNode)] : DefaultWeight;

        public EdgesGraph<T> AddEdgesForNode(T node, T[] edges)
        {
            Edges[node] = edges;
            return this;
        }

        public EdgesGraph<T> AddWeights(T fromNode, T toNode, int weights)
        {
            WeightedNodes[(fromNode, toNode)] = weights;
            return this;
        }
    }
}
