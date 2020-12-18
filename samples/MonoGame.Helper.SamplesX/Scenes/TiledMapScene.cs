using Microsoft.Xna.Framework;
using MonoGame.Helper.ECS;
using MonoGame.Helper.Samples.Systems.TiledMap;

namespace MonoGame.Helper.Samples.Scenes
{
    class TiledMapScene : Scene
    {
        public override void Initialize()
        {
            SetGravity(new Vector2(0f, 58.842f));

            AddSystem<MapSystem>();
            AddSystem<CharacterMovementSystem>();

            base.Initialize();
        }
    }
}
