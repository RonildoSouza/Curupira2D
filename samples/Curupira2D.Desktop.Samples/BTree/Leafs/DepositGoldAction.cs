using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.BehaviorTree.Leafs;
using Curupira2D.Desktop.Samples.Systems.BehaviorTreeAndPathfinder;
using Curupira2D.ECS;

namespace Curupira2D.Desktop.Samples.BTree.Leafs
{
    public class DepositGoldAction(Scene scene) : Leaf
    {
        readonly MinerControllerSystem minerControllerSystem = scene.GetSystem<MinerControllerSystem>();
        float elapsedTime = 0f;

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (minerControllerSystem.MinerState.CurrentMinerAction != MinerState.MinerAction.Idle
                || !minerControllerSystem.MinerState.IsInventoryFull)
                return Failure();

            elapsedTime += scene.DeltaTime;
            if (elapsedTime >= 1)
            {
                blackboard.Remove("NearbyGoldMinePath");
                minerControllerSystem.MinerState.InventoryCapacity = 0;
                elapsedTime = 0f;

                return Success();
            }

            return Running();
        }
    }
}
