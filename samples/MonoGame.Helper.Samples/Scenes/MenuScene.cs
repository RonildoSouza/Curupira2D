using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.Common.Scenes;

namespace MonoGame.Helper.Samples.Scenes
{
    class MenuScene : SceneBase
    {
        public override void Initialize()
        {
            SetTitle("MenuScene");

            ShowText(200, 100, "1 - SpriteAnimationScene\n" +
                               "2 - SceneGraphScene\n" +
                               "3 - CameraScene\n" +
                               "4 - PhysicScene\n" +
                               "5 - TiledMapScene\n");

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.D1))
            {
                GameCore.ChangeScene<SpriteAnimationScene>();
            }

            if (ks.IsKeyDown(Keys.D2))
            {
                GameCore.ChangeScene<SceneGraphScene>();
            }

            if (ks.IsKeyDown(Keys.D3))
            {
                GameCore.ChangeScene<CameraScene>();
            }

            if (ks.IsKeyDown(Keys.D4))
            {
                GameCore.ChangeScene<PhysicScene>();
            }

            if (ks.IsKeyDown(Keys.D5))
            {
                GameCore.ChangeScene<TiledMapScene>();
            }


            base.Update(gameTime);
        }
    }
}
