using Curupira2D.Samples.Common.Scenes;
using Curupira2D.Samples.Systems.TiledMap;

namespace Curupira2D.Samples.Scenes
{
    class TiledMapScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(TiledMapScene));

            AddSystem<MapSystem>();
            AddSystem<CharacterMovementSystem>();

            ShowControlTips("MOVIMENT: Keyboard Arrows");

            base.LoadContent();
        }
    }
}
