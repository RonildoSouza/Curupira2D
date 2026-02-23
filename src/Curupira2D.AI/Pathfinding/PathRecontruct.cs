namespace Curupira2D.AI.Pathfinding
{
    public static class PathRecontruct
    {
        public static IReadOnlyList<T> Execute<T>(Dictionary<T, T> cameFrom, T start, T goal) where T : notnull
        {
            if (!cameFrom.TryGetValue(goal, out _))
                return [];

            var edges = new List<T>(cameFrom.Count / 2);
            var current = goal;

            while (!current.Equals(start))
            {
                edges.Add(current);
                current = cameFrom[current];
            }

            edges.Reverse();
            return edges;
        }
    }
}
