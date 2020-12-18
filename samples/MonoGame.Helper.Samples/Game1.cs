using MonoGame.Helper.Samples.Scenes;

namespace MonoGame.Helper.Samples
{
    public class Game1 : GameCore
    {
        public Game1() : base(800, 640, true) { }

        protected override void Initialize()
        {
            base.Initialize();

            //SetScene<MenuScene>();
            //SetScene<SpriteAnimationScene>();
            //SetScene<SceneGraphScene>();
            //SetScene<CameraScene>();
            //SetScene<PhysicScene>();
            SetScene<TiledMapScene>();
        }
    }
}
