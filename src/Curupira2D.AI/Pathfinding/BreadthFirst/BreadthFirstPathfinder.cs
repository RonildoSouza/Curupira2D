namespace Curupira2D.AI.Pathfinding.BreadthFirst
{
    public static class BreadthFirstPathfinder
    {
        public static Path<T> FindPath<T>(IUnweightedGraph<T> graph, T start, T goal) where T : notnull
        {
            var foundPath = false;
            var frontier = new Queue<T>([start]);
            var cameFrom = new Dictionary<T, T> { { start, start } };

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                // Early Exit
                if (current != null && current.Equals(goal))
                {
                    foundPath = true;
                    break;
                }

                foreach (var next in graph.GetNeighbors(current))
                {
                    if (cameFrom.ContainsKey(next))
                        continue;

                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);
                }
            }

            return new Path<T>(foundPath, cameFrom, PathRecontruct.Execute(cameFrom, start, goal));
        }
    }
}
