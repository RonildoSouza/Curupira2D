using Curupira2D.Samples.Common.Scenes;
using Curupira2D.Samples.Systems.Camera;

namespace Curupira2D.Samples.Scenes
{
    class CameraScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(CameraScene));

            AddSystem<CameraSystem>();

            ShowControlTips("MOVIMENT: Mouse Cursor"
                            + "\nZOOM: Mouse Wheel"
                            + "\nROTATION: Mouse Left Button"
                            + "\nRESET: Mouse Right Button");

            base.LoadContent();
        }
    }
}
