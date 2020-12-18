using MonoGame.Helper.Common.Scenes;
using MonoGame.Helper.Samples.Systems.Physic;

namespace MonoGame.Helper.Samples.Scenes
{
    class PhysicScene : SceneBase
    {
        public override void Initialize()
        {
            SetTitle("PhysicScene");

            AddSystem<BallControllerSystem>();
            AddSystem<SquareControllerSystem>();
            AddSystem<BorderControllerSystem>();

            ShowControlTips(140, 60, "MOVIMENT: Keyboard Arrows");

            base.Initialize();
        }
    }
}
