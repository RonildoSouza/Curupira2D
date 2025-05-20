using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.BehaviorTree.Leafs;
using Curupira2D.Desktop.Samples.Systems.BehaviorTreeAndPathfinder;
using Curupira2D.ECS;
using System;

namespace Curupira2D.Desktop.Samples.BTree.Leafs
{
    public class MinerSleepAction(Scene scene) : Leaf
    {
        private readonly MinerControllerSystem _minerControllerSystem = scene.GetSystem<MinerControllerSystem>();
        private TimeSpan _elapsedTime = TimeSpan.Zero;

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (!_minerControllerSystem.MinerState.IsFatigued)
                return Failure();

            _elapsedTime += scene.ElapsedGameTime;
            if (_minerControllerSystem.MinerState.CurrentMinerAction == MinerState.MinerAction.Idle && _minerControllerSystem.MinerState.IsFatigued)
            {
                _minerControllerSystem.MinerState.CurrentMinerAction = MinerState.MinerAction.Sleep;
                return Running();
            }

            if (_elapsedTime < TimeSpan.FromSeconds(10))
                return Running();

            _minerControllerSystem.MinerState.Energy = 0;
            _minerControllerSystem.MinerState.InventoryCapacity = 0;
            blackboard.Remove("NearbyGoldMine", false);
            _minerControllerSystem.MinerState.CurrentMinerAction = MinerState.MinerAction.Idle;
            _elapsedTime = TimeSpan.Zero;

            return Success();
        }
    }
}
