using Curupira2D.Samples.Common.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Samples.Scenes
{
    class MenuScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(MenuScene));

            ShowText("1 - SpriteAnimationScene\n" +
                     "2 - SceneGraphScene\n" +
                     "3 - CameraScene\n" +
                     "4 - PhysicScene\n" +
                     "5 - TiledMapScene\n" +
                     "6 - Aether Physics2D - HelloWorld\n",
                     ScreenCenter.X,
                     ScreenHeight * 0.8f);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.D1))
                GameCore.ChangeScene<SpriteAnimationScene>();

            if (ks.IsKeyDown(Keys.D2))
                GameCore.ChangeScene<SceneGraphScene>();

            if (ks.IsKeyDown(Keys.D3))
                GameCore.ChangeScene<CameraScene>();

            if (ks.IsKeyDown(Keys.D4))
                GameCore.ChangeScene<PhysicScene>();

            if (ks.IsKeyDown(Keys.D5))
                GameCore.ChangeScene<TiledMapScene>();

            if (ks.IsKeyDown(Keys.D6))
                GameCore.ChangeScene<AetherPhysics2DHelloWorldScene>();

            base.Update(gameTime);
        }
    }
}
