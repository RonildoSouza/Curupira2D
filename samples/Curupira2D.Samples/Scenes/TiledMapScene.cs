using Curupira2D.Testbed.Common.Scenes;
using Curupira2D.Testbed.Systems.TiledMap;

namespace Curupira2D.Testbed.Scenes
{
    class TiledMapScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(TiledMapScene));

            AddSystem<MapSystem>();
            AddSystem<CharacterMovementSystem>();

            ShowControlTips(120, 40, "MOVIMENT: Keyboard Arrows");

            base.LoadContent();
        }
    }
}
