namespace Curupira2D.AI.Pathfinding.AStar
{
    public static class AStarPathfinder
    {
        public static Path<T> FindPath<T>(IAStarGraph<T> graph, T start, T goal) where T : notnull
        {
            var foundPath = false;
            var frontier = new PriorityQueue<T, int>([(start, 0)]);
            var cameFrom = new Dictionary<T, T> { { start, start } };
            var costSoFar = new Dictionary<T, int> { { start, 0 } };

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
                    var newCost = costSoFar[current] + graph.Cost(current, next);

                    if (costSoFar.TryGetValue(next, out int value) && newCost > value)
                        continue;

                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                    frontier.Enqueue(next, newCost + graph.Heuristic(next, goal));
                }
            }

            return new Path<T>(foundPath, cameFrom, PathRecontruct.Execute(cameFrom, start, goal), costSoFar);
        }
    }
}
