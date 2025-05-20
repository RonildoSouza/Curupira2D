using Curupira2D.ECS;

namespace Curupira2D.Desktop.Samples.BTree.Leafs
{
    public class PathfindingGoldDepositAction(Scene scene)
        : BasePathfindingStartingNearbyGoldMineAction(scene, bbPathfindKey: "NearbyGoldMinePathToDeposit", pointObjectName: "gold-deposit")
    { }
}
