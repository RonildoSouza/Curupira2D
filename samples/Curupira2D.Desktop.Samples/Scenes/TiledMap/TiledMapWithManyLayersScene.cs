using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.Desktop.Samples.Systems.Camera;
using Curupira2D.Desktop.Samples.Systems.TiledMap;

namespace Curupira2D.Desktop.Samples.Scenes.TiledMap
{
    class TiledMapWithManyLayersScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(TiledMapWithManyLayersScene));

            AddSystem(new MapSystem("TiledMap/MapWithManyLayers.tmx"));
            AddSystem(new CameraSystem(moveWithKeyboard: true));

            ShowControlTips("MOVIMENT: WASD"
                            + "\nZOOM: Mouse Wheel"
                            + "\nROTATION: Mouse Left Button"
                            + "\nRESET ROTATION: Mouse Right Button",
                            y: 120f);

            base.LoadContent();
        }
    }
}
