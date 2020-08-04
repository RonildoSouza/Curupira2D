using Helper.TiledMap.Systems;
using Microsoft.Xna.Framework;
using MonoGame.Helper;
using MonoGame.Helper.ECS;

namespace Helper.TiledMap
{
    public class Game1 : GameCore
    {
        public Game1() : base(800, 640, true) { }

        protected override void Initialize()
        {
            var _scene = new Scene(new Vector2(0f, 58.842f))
                .AddSystem<MapSystem>()
                .AddSystem<CharacterMovementSystem>();

            SetScene(_scene);

            base.Initialize();
        }
    }
}
