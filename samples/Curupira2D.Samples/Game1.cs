using Curupira2D.Testbed.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Testbed
{
    public class Game1 : GameCore
    {
        public Game1() : base(800, 640, true) { }

        protected override void Initialize()
        {
            base.Initialize();

            AddScene<MenuScene>();
            AddScene<SpriteAnimationScene>();
            AddScene<SceneGraphScene>();
            AddScene<CameraScene>();
            AddScene<PhysicScene>();
            AddScene<TiledMapScene>();

            ChangeScene<MenuScene>();
        }

        protected override void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Q) && !CurrentSceneIs<MenuScene>())
            {
                ChangeScene<MenuScene>();
            }

            base.Update(gameTime);
        }
    }
}
