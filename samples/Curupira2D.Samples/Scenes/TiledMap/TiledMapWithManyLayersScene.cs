using Curupira2D.Samples.Common.Scenes;
using Curupira2D.Samples.Systems.Camera;
using Curupira2D.Samples.Systems.TiledMap;

namespace Curupira2D.Samples.Scenes
{
    class TiledMapWithManyLayersScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(TiledMapWithManyLayersScene));

            AddSystem(new MapSystem("TiledMap/MapWithManyLayers.tmx"));
            AddSystem(new CameraSystem(moveWithKeyboard: true));

            ShowControlTips("MOVIMENT: W, A, S, D"
                            + "\nZOOM: Mouse Wheel"
                            + "\nROTATION: Mouse Left Button"
                            + "\nRESET ROTATION: Mouse Right Button",
                            y: 120f);

            base.LoadContent();
        }
    }
}
