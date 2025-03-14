namespace Curupira2D.AI.Pathfinding.BreadthFirst
{
    public interface IUnweightedGraph<T> where T : notnull
    {
        IEnumerable<T> GetNeighbors(T node);
    }
}
