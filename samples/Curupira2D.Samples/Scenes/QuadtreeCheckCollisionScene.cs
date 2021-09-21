using Curupira2D.Samples.Common.Scenes;
using Curupira2D.Samples.Systems.Quadtree;

namespace Curupira2D.Samples.Scenes
{
    class QuadtreeCheckCollisionScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(QuadtreeCheckCollisionScene));

            AddSystem<QuadtreeCheckCollisionSystem>();

            ShowControlTips("MOVIMENT: Keyboard Arrows");

            base.LoadContent();
        }
    }
}
