using Curupira2D.Console.Samples.AI;

while (true)
{
    Console.WriteLine("\n\nCHOOSE AN OPTION:");
    Console.WriteLine("1 - PATHFINDING");
    Console.WriteLine("2 - BEHAVIOR TREE");

    Console.WriteLine("\nQ - Exit");
    Console.Write("\n>>> ");

    var option = Console.ReadLine();
    switch (option)
    {
        case "1":
            Console.Clear();
            PathfindingTest.Main();
            break;
        case "2":
            Console.Clear();
            BehaviorTreeTest.Main();
            break;
        case "Q" or "q": return;
        default: Console.WriteLine("Invalid option."); break;
    }
}
