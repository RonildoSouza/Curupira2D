using Microsoft.Xna.Framework;
using MonoGame.Helper.Common.Scenes;
using MonoGame.Helper.Samples.Systems.TiledMap;

namespace MonoGame.Helper.Samples.Scenes
{
    class TiledMapScene : SceneBase
    {
        public override void Initialize()
        {
            SetTitle("TiledMapScene");

            SetGravity(new Vector2(0f, 58.842f));

            AddSystem<MapSystem>();
            AddSystem<CharacterMovementSystem>();

            ShowControlTips(120, 40, "MOVIMENT: Keyboard Arrows");

            base.Initialize();
        }
    }
}
