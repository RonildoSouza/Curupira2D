using Curupira2D.Samples.Common.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Samples.Scenes
{
    class MenuScene : SceneBase
    {
        KeyboardState _oldKeyState;

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
            KeyboardInputManager.Begin();

            if (KeyboardInputManager.IsKeyPressed(Keys.D1))
                GameCore.ChangeScene<SpriteAnimationScene>();

            if (KeyboardInputManager.IsKeyPressed(Keys.D2))
                GameCore.ChangeScene<SceneGraphScene>();

            if (KeyboardInputManager.IsKeyPressed(Keys.D3))
                GameCore.ChangeScene<CameraScene>();

            if (KeyboardInputManager.IsKeyPressed(Keys.D4))
                GameCore.ChangeScene<PhysicScene>();

            if (KeyboardInputManager.IsKeyPressed(Keys.D5))
                GameCore.ChangeScene<TiledMapScene>();

            if (KeyboardInputManager.IsKeyPressed(Keys.D6))
                GameCore.ChangeScene<AetherPhysics2DHelloWorldScene>();

            KeyboardInputManager.End();

            base.Update(gameTime);
        }
    }
}
