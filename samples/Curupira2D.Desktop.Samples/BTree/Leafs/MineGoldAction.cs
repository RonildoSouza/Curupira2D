using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.BehaviorTree.Leafs;
using Curupira2D.Desktop.Samples.Systems.BehaviorTreeAndPathfinder;
using Curupira2D.ECS;
using System;

namespace Curupira2D.Desktop.Samples.BTree.Leafs
{
    public class MineGoldAction(Scene scene) : Leaf
    {
        readonly MinerControllerSystem minerControllerSystem = scene.GetSystem<MinerControllerSystem>();
        float elapsedTime = 0f;

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (minerControllerSystem.MinerState.CurrentMinerAction != MinerState.MinerAction.Mine)
                return Failure();

            elapsedTime += scene.DeltaTime;
            if (elapsedTime >= 1)
            {
                minerControllerSystem.MinerState.Energy = minerControllerSystem.MinerState.Energy + Random.Shared.Next(0, 4);
                minerControllerSystem.MinerState.InventoryCapacity++;
                elapsedTime = 0f;
            }

            if (minerControllerSystem.MinerState.IsInventoryFull)
            {
                minerControllerSystem.MinerState.CurrentMinerAction = MinerState.MinerAction.GoHome;
                return Success();
            }

            return Running();
        }
    }
}
