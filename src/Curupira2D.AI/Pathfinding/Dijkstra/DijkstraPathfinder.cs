using System.Diagnostics;

namespace Curupira2D.AI.Pathfinding.Dijkstra
{
    public static class DijkstraPathfinder
    {
        private const int DefaultCapacity = 256;
        private static readonly long TicksPerTimeSpanTick = Stopwatch.Frequency / TimeSpan.TicksPerSecond;

        public static Path<T> FindPath<T>(IWeightedGraph<T> graph, T start, T goal, TimeSpan timeout = default) where T : notnull
        {
            if (start.Equals(goal))
                return new Path<T>(true, new Dictionary<T, T> { { start, start } }, [start], new Dictionary<T, int> { { start, 0 } });

            var timeoutTicks = (timeout == default ? TimeSpan.FromSeconds(30) : timeout).Ticks;
            var startTimestamp = Stopwatch.GetTimestamp();

            var foundPath = false;
            var frontier = new PriorityQueue<T, int>(DefaultCapacity);
            var cameFrom = new Dictionary<T, T>(DefaultCapacity) { { start, start } };
            var costSoFar = new Dictionary<T, int>(DefaultCapacity) { { start, 0 } };
            var closedSet = new HashSet<T>(DefaultCapacity);

            frontier.Enqueue(start, 0);

            while (frontier.Count > 0)
            {
                var elapsedTicks = (Stopwatch.GetTimestamp() - startTimestamp) / TicksPerTimeSpanTick;
                if (elapsedTicks >= timeoutTicks)
                    break;

                var current = frontier.Dequeue();

                // Early exit
                if (current.Equals(goal))
                {
                    foundPath = true;
                    break;
                }

                if (!closedSet.Add(current))
                    continue;

                foreach (var next in graph.GetNeighbors(current))
                {
                    if (closedSet.Contains(next))
                        continue;

                    var newCost = costSoFar[current] + graph.Cost(current, next);

                    if (costSoFar.TryGetValue(next, out int existingCost) && newCost >= existingCost)
                        continue;

                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                    frontier.Enqueue(next, newCost);
                }
            }

            return new Path<T>(foundPath, cameFrom, PathRecontruct.Execute(cameFrom, start, goal), costSoFar);
        }
    }
}
