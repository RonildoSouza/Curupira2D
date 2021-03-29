using Curupira2D.Testbed.Common.Scenes;
using Curupira2D.Testbed.Systems.Camera;

namespace Curupira2D.Testbed.Scenes
{
    class CameraScene : SceneBase
    {
        public override void Initialize()
        {
            SetTitle(nameof(CameraScene));

            AddSystem<CameraSystem>();

            ShowControlTips(140, 80, "MOVIMENT: Mouse Cursor"
                                    + "\nZOOM: Mouse Wheel"
                                    + "\nROTATION: Mouse Left Button"
                                    + "\nRESET: Mouse Right Button");

            base.Initialize();
        }
    }
}
