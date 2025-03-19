using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.Desktop.Samples.Systems.TiledMap;

namespace Curupira2D.Desktop.Samples.Scenes
{
    class BehaviorTreeAndPathfinderScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(BehaviorTreeAndPathfinderScene));

            AddSystem(new MapSystem("AI/BehaviorTreeAndPathfinderTiledMap.tmx", "AI/BehaviorTreeAndPathfinderTileset"));

            ShowControlTips("MOVIMENT: Keyboard Arrows", y: 120f);

            base.LoadContent();
        }
    }
}
