using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.BehaviorTree.Leafs;
using Curupira2D.Desktop.Samples.Systems.BehaviorTreeAndPathfinder;
using Curupira2D.ECS;

namespace Curupira2D.Desktop.Samples.BTree.Conditions
{
    public class ThereIsGoldAvailableCondition(Scene scene) : Leaf
    {
        private readonly GoldMineControllerSystem _goldMineControllerSystem = scene.GetSystem<GoldMineControllerSystem>();

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (_goldMineControllerSystem == null)
                return Failure();

            var entityUniqueId = blackboard.Get<string>("NearbyGoldMineEntityUniqueId");
            return _goldMineControllerSystem.ThereIsGoldAvailable(entityUniqueId) ? Success() : Failure();
        }
    }
}
