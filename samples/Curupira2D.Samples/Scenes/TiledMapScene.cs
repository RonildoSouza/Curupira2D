using Curupira2D.Testbed.Common.Scenes;
using Curupira2D.Testbed.Systems.TiledMap;
using Microsoft.Xna.Framework;

namespace Curupira2D.Testbed.Scenes
{
    class TiledMapScene : SceneBase
    {
        public override void Initialize()
        {
            SetTitle(nameof(TiledMapScene));

            SetGravity(new Vector2(0f, 58.842f));

            AddSystem<MapSystem>();
            AddSystem<CharacterMovementSystem>();

            ShowControlTips(120, 40, "MOVIMENT: Keyboard Arrows");

            base.Initialize();
        }
    }
}
