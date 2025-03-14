using Curupira2D.AI.Extensions;
using Curupira2D.AI.Pathfinding.AStar;
using Curupira2D.AI.Pathfinding.BreadthFirst;
using Curupira2D.AI.Pathfinding.Dijkstra;
using Curupira2D.AI.Pathfinding.Graphs;
using System.Drawing;

var _width = 30;
var _height = 15;
var _allowDiagonalSearch = false;
var _walls = new HashSet<Point>([
    new Point(3, 3),
    new Point(3, 4),
    new Point(3, 5),
    new Point(3, 6),
    new Point(3, 7),
    new Point(3, 8),
    new Point(3, 9),
    new Point(3, 10),
    new Point(3, 11),

    new Point(12, 4),
    new Point(12, 5),
    new Point(12, 6),
    new Point(12, 7),
    new Point(12, 8),
    new Point(12, 9),
    new Point(12, 10),
    new Point(12, 11),
    new Point(12, 12),
    new Point(12, 13),
    new Point(12, 14),
    new Point(12, 15),

    new Point(20, 0),
    new Point(20, 1),
    new Point(20, 2),
    new Point(20, 3),
    new Point(20, 4),
    new Point(20, 5),
    new Point(20, 6),
    new Point(21, 5),
    new Point(21, 6),
    new Point(22, 5),
    new Point(22, 6),
    new Point(23, 5),
    new Point(23, 6),
]);
var _start = new Point(7, 8);
var _goal = new Point(26, 6);

BreadthFirstSearchGridGraphTest(_width, _height, _allowDiagonalSearch, _walls, _start, _goal);

BreadthFirstSearchEdgesGraphTest();

DijkstraSearchTest(_width, _height, _allowDiagonalSearch, _walls, _start, _goal);

AStarSearchGridGraphTest(_width, _height, _allowDiagonalSearch, _walls, _start, _goal);

AStarSearchEdgesGraphTest();



static void BreadthFirstSearchGridGraphTest(int width, int height, bool allowDiagonalSearch, HashSet<Point> walls, Point start, Point goal)
{
    Console.WriteLine("\n*** BREADTH FIRST SEARCH - GRID GRAPH ***");

    var gridGraph = new GridGraph(width, height, allowDiagonalSearch)
    {
        Walls = walls
    };

    var path = BreadthFirstPathfinder.FindPath(gridGraph, start, goal);

    gridGraph.WriteLine(start, goal, path);
    gridGraph.WriteLine(start, goal, path, showPath: true, showCostSoFar: false);
}

static void BreadthFirstSearchEdgesGraphTest()
{
    Console.WriteLine("\n*** BREADTH FIRST SEARCH - EDGES GRAPH ***");

    var edgesGraph = new EdgesGraph<string>()
        .AddEdgesForNode("A", ["B"])
        .AddEdgesForNode("B", ["A", "C", "D"])
        .AddEdgesForNode("C", ["A"])
        .AddEdgesForNode("D", ["E", "A"])
        .AddEdgesForNode("E", ["B"]);
    var start = "B";
    var goal = "E";

    var path = BreadthFirstPathfinder.FindPath(edgesGraph, start, goal);
    edgesGraph.WriteLine(start, goal, path);
}

static void DijkstraSearchTest(int width, int height, bool allowDiagonalSearch, HashSet<Point> walls, Point start, Point goal)
{
    Console.WriteLine("\n*** DIJKSTRA SEARCH ***");

    var gridGraph = new GridGraph(width, height, allowDiagonalSearch)
    {
        Walls = walls
    };

    var path = DijkstraPathfinder.FindPath(gridGraph, start, goal);

    gridGraph.WriteLine(start, goal, path);
    gridGraph.WriteLine(start, goal, path, showPath: true, showCostSoFar: false);
}

static void AStarSearchGridGraphTest(int width, int height, bool allowDiagonalSearch, HashSet<Point> walls, Point start, Point goal)
{
    Console.WriteLine("\n*** A* SEARCH - GRID GRAPH ***");

    var gridGraph = new GridGraph(width, height, allowDiagonalSearch)
    {
        Walls = walls,
        //DefaultWeight = 2,
    };

    var path = AStarPathfinder.FindPath(gridGraph, start, goal);

    gridGraph.WriteLine(start, goal, path);
    gridGraph.WriteLine(start, goal, path, showPath: true, showCostSoFar: false);
}

static void AStarSearchEdgesGraphTest()
{
    Console.WriteLine("\n*** A* SEARCH - EDGES GRAPH ***");

    var edgesGraph = new EdgesPointGraph()
        .AddEdgesForNode(new Point(10, 10), [new Point(10, 30)])
        .AddEdgesForNode(new Point(10, 30), [new Point(10, 40), new Point(30, 60)])
        .AddEdgesForNode(new Point(10, 40), [])
        .AddEdgesForNode(new Point(30, 60), [new Point(10, 40)]);
    var start = new Point(10, 10);
    var goal = new Point(10, 40);

    var path = AStarPathfinder.FindPath(edgesGraph, start, goal);

    edgesGraph.WriteLine(start, goal, path);
}
