namespace Curupira2D.AI.Pathfinding
{
    public class Path<T>(bool foundPath, Dictionary<T, T> cameFrom, IReadOnlyList<T> edges, Dictionary<T, int>? costSoFar = null) where T : notnull
    {
        public bool FoundPath => foundPath;
        public IReadOnlyDictionary<T, T> CameFrom => cameFrom;
        public IReadOnlyList<T> Edges => edges;
        public IReadOnlyDictionary<T, int> CostSoFar => costSoFar ?? [];
        public int DurationCostSoFar => CostSoFar.Values.Sum();
    }
}
