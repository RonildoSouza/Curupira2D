using MonoGame.Helper.Common.Scenes;
using MonoGame.Helper.Samples.Systems.Camera;

namespace MonoGame.Helper.Samples.Scenes
{
    class CameraScene : SceneBase
    {
        public override void Initialize()
        {
            SetTitle("CameraScene");

            AddSystem<CameraSystem>();

            ShowControlTips(140, 80, "MOVIMENT: Mouse Cursor"
                                    + "\nZOOM: Mouse Wheel"
                                    + "\nROTATION: Mouse Left Button"
                                    + "\nROTATION: Mouse Right Button");

            base.Initialize();
        }
    }
}
