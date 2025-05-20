using Curupira2D.AI.BehaviorTree;
using Curupira2D.Desktop.Samples.BTree.Conditions;
using Curupira2D.Desktop.Samples.BTree.Leafs;
using Curupira2D.ECS;
using Curupira2D.ECS.Components;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;

namespace Curupira2D.Desktop.Samples.Systems.BehaviorTreeAndPathfinder
{
    [RequiredComponent(typeof(BehaviorTreeMinerControllerSystem), typeof(DumpComponent))]
    public class BehaviorTreeMinerControllerSystem(IBlackboard blackboard) : ECS.System, ILoadable, IUpdatable
    {
        BehaviorTree _behaviorTree;

        public void LoadContent()
        {
            var minerControllerSystem = Scene.GetSystem<MinerControllerSystem>();

            _behaviorTree = BehaviorTreeBuilder.GetInstance()
                .Selector()
                    .Leaf<PathfindingNearbyGoldMineAction>(Scene)
                    .Sequence()
                        .Conditional(bb => minerControllerSystem.MinerState.IsFatigued)
                        .Leaf<PathfindingHomeAction>(Scene)
                        .Leaf<MoveToHomeAction>(Scene)
                        .Leaf<MinerSleepAction>(Scene)
                    .Close()
                    .Sequence()
                        .Inverter()
                            .Leaf<HasFreeSpaceInInventoryCondition>(Scene)
                        .Leaf<PathfindingGoldDepositAction>(Scene)
                        .Leaf<MoveToGoldDepositAction>(Scene)
                        .Leaf<DepositGoldAction>(Scene)
                    .Close()
                    .Sequence()
                        .Leaf<ThereIsGoldAvailableCondition>(Scene)
                        .Leaf<HasFreeSpaceInInventoryCondition>(Scene)
                        .Leaf<MoveToGoldMineAction>(Scene)
                        .Leaf<MineGoldAction>(Scene)
                    .Close()
                .Close()
            .Build(blackboard);

            //System.Diagnostics.Debug.WriteLine(_behaviorTree.GetTreeStructure());
        }

        public void Update()
        {
            _behaviorTree?.Tick();
            //System.Diagnostics.Debug.WriteLine(_behaviorTree.GetTreeStructure(true));
        }
    }
}
