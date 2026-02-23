using System.Diagnostics;

namespace Curupira2D.AI.Pathfinding.BreadthFirst
{
    public static class BreadthFirstPathfinder
    {
        private const int DefaultCapacity = 256;
        private static readonly long TicksPerTimeSpanTick = Stopwatch.Frequency / TimeSpan.TicksPerSecond;

        public static Path<T> FindPath<T>(IUnweightedGraph<T> graph, T start, T goal, TimeSpan timeout = default) where T : notnull
        {
            if (start.Equals(goal))
                return new Path<T>(true, new Dictionary<T, T> { { start, start } }, [start]);

            var timeoutTicks = (timeout == default ? TimeSpan.FromSeconds(30) : timeout).Ticks;
            var startTimestamp = Stopwatch.GetTimestamp();

            var foundPath = false;
            var frontier = new Queue<T>(DefaultCapacity);
            var cameFrom = new Dictionary<T, T>(DefaultCapacity) { { start, start } };

            frontier.Enqueue(start);

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

                foreach (var next in graph.GetNeighbors(current))
                {
                    if (!cameFrom.TryAdd(next, current))
                        continue;

                    frontier.Enqueue(next);
                }
            }

            return new Path<T>(foundPath, cameFrom, PathRecontruct.Execute(cameFrom, start, goal));
        }
    }
}
