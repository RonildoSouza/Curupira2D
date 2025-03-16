using Curupira2D.AI.Pathfinding;
using Curupira2D.AI.Pathfinding.Graphs;
using System.Drawing;

namespace Curupira2D.AI.Extensions
{
    public static class GraphExtensions
    {
        public static void WriteLine(this GridGraph graph, Point start, Point goal, Path<Point> path, bool showPath = false, bool showCostSoFar = false)
        {
            const int fieldWidth = 3;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            if (!showPath)
                Console.WriteLine($"* FOUND PATH: {path.FoundPath}");

            Console.WriteLine(new string('_', fieldWidth * graph.Width));

            for (int y = 0; y < graph.Height; y++)
            {
                for (int x = 0; x < graph.Width; x++)
                {
                    var node = new Point(x, y);

                    if (graph.Walls.Contains(node))
                        Console.Write(new string('#', fieldWidth)); // WALLS
                    else if (start == node)
                        Console.Write(" S "); // START
                    else if (goal == node)
                        Console.Write(" G "); // GOAL
                    else if (showPath && path.Edges != null && path.Edges.Contains(node))
                        Console.Write(" @ "); // PATH
                    else if (!showPath && path.CameFrom != null && path.CameFrom.TryGetValue(node, out Point next))
                    {
                        if (next.X == x + 1)
                            Console.Write(" → ");
                        else if (next.X == x - 1)
                            Console.Write(" ← ");
                        else if (next.Y == y + 1)
                            Console.Write(" ↓ ");
                        else if (next.Y == y - 1)
                            Console.Write(" ↑ ");
                        else
                            Console.Write(" * ");
                    }
                    else if (showPath && showCostSoFar && path.CostSoFar != null && path.CostSoFar.TryGetValue(node, out int value))
                        Console.Write($"{value} ".PadLeft(3, '0'));
                    else
                        Console.Write(" . ");
                }

                Console.WriteLine();
            }

            Console.WriteLine(new string('_', fieldWidth * graph.Width));
        }

        public static void WriteLine<T>(this EdgesGraph<T> graph, T start, T goal, Path<T> path)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine($"* FROM: {start} TO: {goal}");
            Console.WriteLine($"* FOUND PATH: {path.FoundPath}");
            Console.WriteLine($"* CAME FROM: {string.Join(" → ", path.CameFrom.Keys)}");
            Console.WriteLine();

            foreach (var node in graph.Edges)
            {
                var value = string.Empty;
                value += $"{node.Key}";

                foreach (var edge in node.Value)
                    value += $" → {edge}";

                Console.WriteLine(value);
            }
        }
    }
}
