using Curupira2D.Samples.Common.Scenes;
using Curupira2D.Samples.Systems.TiledMap;

namespace Curupira2D.Samples.Scenes
{
    class PlatformerTiledMapScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(PlatformerTiledMapScene));

            AddSystem(new MapSystem("TiledMap/PlatformerTiledMap.tmx", "TiledMap/PlatformerTileset"));
            AddSystem<CharacterMovementSystem>();

            ShowControlTips("MOVIMENT: Keyboard Arrows OR WASD");

            base.LoadContent();
        }
    }
}
