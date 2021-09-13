using Curupira2D.Samples.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Samples
{
    public class Game1 : GameCore
    {
        public Game1() : base(width: 800, height: 640, debugActive: true) { }

        protected override void LoadContent()
        {
            AddScene<MenuScene>();
            AddScene<SceneTest>();
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
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Q) && !CurrentSceneIs<MenuScene>())
            {
                ChangeScene<MenuScene>();
            }

            base.Update(gameTime);
        }
    }
}
