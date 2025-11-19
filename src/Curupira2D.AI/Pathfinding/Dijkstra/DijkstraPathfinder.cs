using System.Diagnostics;

namespace Curupira2D.AI.Pathfinding.Dijkstra
{
    public static class DijkstraPathfinder
    {
        public static Path<T> FindPath<T>(IWeightedGraph<T> graph, T start, T goal, TimeSpan timeout = default) where T : notnull
        {
            var foundPath = false;
            var frontier = new PriorityQueue<T, int>([(start, 0)]);
            var cameFrom = new Dictionary<T, T> { { start, start } };
            var costSoFar = new Dictionary<T, int> { { start, 0 } };

            if (timeout == default)
                timeout = TimeSpan.FromSeconds(30);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (frontier.Count > 0 && stopwatch.Elapsed <= timeout)
            {
                var current = frontier.Dequeue();

                if (current == null)
                    break;

                // Early Exit
                if (current.Equals(goal))
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
                    frontier.Enqueue(next, newCost);
                }
            }

            stopwatch.Stop();

            return new Path<T>(foundPath, cameFrom, PathRecontruct.Execute(cameFrom, start, goal), costSoFar);
        }
    }
}
