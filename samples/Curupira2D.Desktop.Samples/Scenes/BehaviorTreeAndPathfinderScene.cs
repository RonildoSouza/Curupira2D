using Curupira2D.AI.BehaviorTree;
using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.Desktop.Samples.Systems.BehaviorTreeAndPathfinder;
using Curupira2D.Desktop.Samples.Systems.TiledMap;

namespace Curupira2D.Desktop.Samples.Scenes
{
    class BehaviorTreeAndPathfinderScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(BehaviorTreeAndPathfinderScene));

            var blackboard = new Blackboard();

            AddSystem<MinerControllerSystem>(blackboard);
            AddSystem<BehaviorTreeMinerControllerSystem>(blackboard);
            AddSystem<GoldMineControllerSystem>();
            AddSystem(new MapSystem("AI/BehaviorTreeAndPathfinderTiledMap.tmx", "AI/BehaviorTreeAndPathfinderTileset"));

            base.LoadContent();
        }
    }
}
