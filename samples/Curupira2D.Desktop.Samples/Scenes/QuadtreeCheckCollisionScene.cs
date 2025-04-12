using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.Desktop.Samples.Systems.Quadtree;

namespace Curupira2D.Desktop.Samples.Scenes
{
    class QuadtreeCheckCollisionScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(QuadtreeCheckCollisionScene));

            AddSystem<QuadtreeCheckCollisionSystem>();

            ShowControlTips("MOVIMENT: Keyboard Arrows OR WASD");

            base.LoadContent();
        }
    }
}
