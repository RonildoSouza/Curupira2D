using Curupira2D.Samples.Common.Scenes;
using Curupira2D.Samples.Systems.Camera;
using Curupira2D.Samples.Systems.TiledMap;
using Microsoft.Xna.Framework;

namespace Curupira2D.Samples.Scenes
{
    class IsometricTiledMapScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(IsometricTiledMapScene));

            SetCleanColor(Color.Black);

            // Approximate downward force in isometric space
            Gravity = new Vector2(4.9f, 4.9f);

            AddSystem<IsometricCharacterAnimationSystem>();
            AddSystem(new MapSystem("TiledMap/IsometricTiledMap.tmx", "TiledMap/IsometricCity"));
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
