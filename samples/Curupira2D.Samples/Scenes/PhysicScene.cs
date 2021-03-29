using Curupira2D.Testbed.Common.Scenes;
using Curupira2D.Testbed.Systems.Physic;

namespace Curupira2D.Testbed.Scenes
{
    class PhysicScene : SceneBase
    {
        public override void Initialize()
        {
            SetTitle(nameof(PhysicScene));

            AddSystem<BallControllerSystem>();
            AddSystem<SquareControllerSystem>();
            AddSystem<BorderControllerSystem>();

            ShowControlTips(140, 60, "MOVIMENT: Keyboard Arrows");

            base.Initialize();
        }
    }
}
