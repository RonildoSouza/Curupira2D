using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.Desktop.Samples.Systems.TiledMap;

namespace Curupira2D.Desktop.Samples.Scenes.TiledMap
{
    class IsometricTiledMapScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(IsometricTiledMapScene));

            AddSystem<IsometricBicycleAnimationSystem>();
            AddSystem(new MapSystem("TiledMap/IsometricTiledMap.tmx", "TiledMap/IsometricCity"));

            ShowControlTips("MOVIMENT: Keyboard Arrows", y: 120f);

            base.LoadContent();
        }
    }
}
