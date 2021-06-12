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
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.D1) && _oldKeyState.IsKeyUp(Keys.D1))
                GameCore.ChangeScene<SpriteAnimationScene>();

            if (keyState.IsKeyDown(Keys.D2) && _oldKeyState.IsKeyUp(Keys.D2))
                GameCore.ChangeScene<SceneGraphScene>();

            if (keyState.IsKeyDown(Keys.D3) && _oldKeyState.IsKeyUp(Keys.D3))
                GameCore.ChangeScene<CameraScene>();

            if (keyState.IsKeyDown(Keys.D4) && _oldKeyState.IsKeyUp(Keys.D4))
                GameCore.ChangeScene<PhysicScene>();

            if (keyState.IsKeyDown(Keys.D5) && _oldKeyState.IsKeyUp(Keys.D5))
                GameCore.ChangeScene<TiledMapScene>();

            if (keyState.IsKeyDown(Keys.D6) && _oldKeyState.IsKeyUp(Keys.D6))
                GameCore.ChangeScene<AetherPhysics2DHelloWorldScene>();

            _oldKeyState = keyState;

            base.Update(gameTime);
        }
    }
}
