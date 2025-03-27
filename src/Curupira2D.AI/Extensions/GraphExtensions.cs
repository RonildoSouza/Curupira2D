using Curupira2D.AI.Pathfinding;
using Curupira2D.AI.Pathfinding.Graphs;
using System.Drawing;
using System.Text;

namespace Curupira2D.AI.Extensions
{
    public static class GraphExtensions
    {
        public static void WriteLine(this GridGraph graph, Point start, Point goal, Path<Point> path, bool showPath = false, bool showCostSoFar = false)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(graph.GetDebugPathfinder(start, goal, path, showPath, showCostSoFar));
        }

        public static void WriteLine<T>(this EdgesGraph<T> graph, T start, T goal, Path<T> path) where T : notnull
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(graph.GetDebugPathfinder<T>(start, goal, path));
        }

        public static string GetDebugPathfinder(this GridGraph graph, Point start, Point goal, Path<Point> path, bool showPath = false, bool showCostSoFar = false)
        {
            const int fieldWidth = 3;
            var sb = new StringBuilder();

            if (!showPath)
                sb.AppendLine($"* FOUND PATH: {path.FoundPath}");

            sb.AppendLine(new string('_', fieldWidth * graph.Width));

            for (int y = 0; y < graph.Height; y++)
            {
                for (int x = 0; x < graph.Width; x++)
                {
                    var node = new Point(x, y);

                    if (graph.Walls.Contains(node))
                        sb.Append(new string('#', fieldWidth)); // WALLS
                    else if (start == node)
                        sb.Append(" S "); // START
                    else if (goal == node)
                        sb.Append(" G "); // GOAL
                    else if (showPath && path.Edges != null && path.Edges.Contains(node))
                        sb.Append(" @ "); // PATH
                    else if (!showPath && path.CameFrom != null && path.CameFrom.TryGetValue(node, out Point next))
                    {
                        if (next.X == x + 1)
                            sb.Append(" → ");
                        else if (next.X == x - 1)
                            sb.Append(" ← ");
                        else if (next.Y == y + 1)
                            sb.Append(" ↓ ");
                        else if (next.Y == y - 1)
                            sb.Append(" ↑ ");
                        else
                            sb.Append(" * ");
                    }
                    else if (showPath && showCostSoFar && path.CostSoFar != null && path.CostSoFar.TryGetValue(node, out int value))
                        sb.Append($"{value} ".PadLeft(3, '0'));
                    else
                        sb.Append(" . ");
                }

                sb.AppendLine();
            }

            sb.AppendLine(new string('_', fieldWidth * graph.Width));

            return sb.ToString();
        }

        public static string GetDebugPathfinder<T>(this EdgesGraph<T> graph, T start, T goal, Path<T> path)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"* FROM: {start} TO: {goal}");
            sb.AppendLine($"* FOUND PATH: {path.FoundPath}");
            sb.AppendLine($"* CAME FROM: {string.Join(" → ", path.CameFrom.Keys)}");
            sb.AppendLine();

            foreach (var node in graph.Edges)
            {
                var value = string.Empty;
                value += $"{node.Key}";

                foreach (var edge in node.Value)
                    value += $" → {edge}";

                sb.AppendLine(value);
            }

            return sb.ToString();
        }
    }
}
