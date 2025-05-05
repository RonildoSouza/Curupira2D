using Curupira2D.AI.BehaviorTree;
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
                        .Leaf<FindingNearbyGoldMineAction>(Scene)
                    .Sequence()
                        .Conditional(bb => minerControllerSystem.MinerState.IsFatigued)
                        //.DebugLogAction("Go home")
                        //.DebugLogAction("Sleep")
                    .Close()
                    .Sequence()
                        .Conditional(bb => minerControllerSystem.MinerState.IsInventoryFull)
                        .Leaf<MoveToHomeAction>(Scene)
                        //.DebugLogAction("Go home")
                        //.DebugLogAction("Deposit gold")
                    .Close()
                    .Sequence()
                        .Conditional(bb => !minerControllerSystem.MinerState.IsInventoryFull)
                        //.DebugLogAction("Mine has gold available?")
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
