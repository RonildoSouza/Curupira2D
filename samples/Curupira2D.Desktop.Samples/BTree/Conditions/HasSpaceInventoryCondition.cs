using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.BehaviorTree.Leafs;
using Curupira2D.Desktop.Samples.Systems.BehaviorTreeAndPathfinder;
using Curupira2D.ECS;

namespace Curupira2D.Desktop.Samples.BTree.Conditions
{
    public class HasSpaceInventoryCondition(Scene scene) : Leaf
    {
        readonly MinerControllerSystem minerControllerSystem = scene.GetSystem<MinerControllerSystem>();

        public override BehaviorState Update(IBlackboard blackboard)
            => minerControllerSystem.MinerState.IsInventoryFull ? Failure() : Success();
    }
}
