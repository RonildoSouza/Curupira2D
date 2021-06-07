using Curupira2D.Testbed.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Testbed
{
    public class Game1 : GameCore
    {
        public Game1() : base(width: 800, height: 640, debugActive: true)
        {
            Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            AddScene<MenuScene>();
            AddScene<SpriteAnimationScene>();
            AddScene<SceneGraphScene>();
            AddScene<CameraScene>();
            AddScene<PhysicScene>();
            AddScene<TiledMapScene>();
            AddScene<AetherPhysics2DHelloWorldScene>();

            ChangeScene<MenuScene>();

            base.LoadContent();
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
