namespace Curupira2D.AI.Pathfinding.Dijkstra
{
    public interface IWeightedGraph<T> where T : notnull
    {
        IEnumerable<T> GetNeighbors(T node);
        int Cost(T fromNode, T toNode);
    }
}
