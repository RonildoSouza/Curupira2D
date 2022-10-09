using Curupira2D.Samples.Common.Scenes;
using Curupira2D.Samples.Systems.Physic;

namespace Curupira2D.Samples.Scenes
{
    class PhysicScene : SceneBase
    {
        public override void LoadContent()
        {

            SetTitle(nameof(PhysicScene));

            AddSystem<BallControllerSystem>();
            AddSystem<SquareControllerSystem>();
            AddSystem<BorderControllerSystem>();

            ShowControlTips("MOVIMENT: Keyboard Arrows OR WASD", x: 200f, y: 120f);

            base.LoadContent();
        }
    }
}
