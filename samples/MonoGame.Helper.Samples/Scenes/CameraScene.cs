using MonoGame.Helper.ECS;
using MonoGame.Helper.Samples.Systems.Camera;

namespace MonoGame.Helper.Samples.Scenes
{
    class CameraScene : Scene
    {
        public override void Initialize()
        {
            SetTitle("CameraScene");

            AddSystem<CameraSystem>();

            base.Initialize();
        }
    }
}
