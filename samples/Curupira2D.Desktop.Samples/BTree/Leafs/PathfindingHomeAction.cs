using Curupira2D.ECS;

namespace Curupira2D.Desktop.Samples.BTree.Leafs
{
    public class PathfindingHomeAction(Scene scene)
        : BasePathfindingStartingNearbyGoldMineAction(scene, bbPathfindKey: "NearbyGoldMinePathToHome", pointObjectName: "home")
    { }
}
