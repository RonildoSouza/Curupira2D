using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.BehaviorTree.Leafs;
using Curupira2D.Desktop.Samples.Systems.BehaviorTreeAndPathfinder;
using Curupira2D.ECS;
using System;

namespace Curupira2D.Desktop.Samples.BTree.Leafs
{
    public class DepositGoldAction(Scene scene) : Leaf
    {
        private readonly MinerControllerSystem _minerControllerSystem = scene.GetSystem<MinerControllerSystem>();
        private TimeSpan _elapsedTime = TimeSpan.Zero;

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (_minerControllerSystem.MinerState.CurrentMinerAction != MinerState.MinerAction.Idle
                || !_minerControllerSystem.MinerState.IsInventoryFull)
                return Failure();

            _elapsedTime += scene.ElapsedGameTime;
            if (_elapsedTime >= TimeSpan.FromSeconds(0.3))
            {
                blackboard.Remove("NearbyGoldMine", false);
                _minerControllerSystem.MinerState.InventoryCapacity = 0;
                _elapsedTime = TimeSpan.Zero;

                return Success();
            }

            return Running();
        }
    }
}
