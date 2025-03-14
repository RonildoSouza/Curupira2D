namespace Curupira2D.AI.Pathfinding.AStar
{
    public interface IAStarGraph<T> where T : notnull
    {
        IEnumerable<T> GetNeighbors(T node);
        int Cost(T fromNode, T toNode);
        int Heuristic(T node, T goal);
    }
}
