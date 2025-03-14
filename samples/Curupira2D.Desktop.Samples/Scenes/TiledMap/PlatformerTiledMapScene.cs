using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.Desktop.Samples.Systems.TiledMap;

namespace Curupira2D.Desktop.Samples.Scenes.TiledMap
{
    class PlatformerTiledMapScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(PlatformerTiledMapScene));

            AddSystem(new MapSystem("TiledMap/PlatformerTiledMap.tmx", "TiledMap/PlatformerTileset"));
            AddSystem<CharacterMovementSystem>();

            ShowControlTips("MOVIMENT: Keyboard Arrows OR WASD", y: 120f);

            base.LoadContent();
        }
    }
}
