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

            ShowText("1 - Sprite Animation\n" +
                     "2 - Scene Graph\n" +
                     "3 - Camera 2D\n" +
                     "4 - Physic\n" +
                     "5 - Platformer Tiled Map\n" +
                     "6 - Aether Physics2D - HelloWorld\n" +
                     "7 - Quadtree Check Collision\n" +
                     "8 - Tiled Map With Many Layers\n",
                     ScreenCenter.X,
                     ScreenCenter.Y,
                     scale: Vector2.One);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardInputManager.Begin();

            if (KeyboardInputManager.IsKeyPressed(Keys.T))
                GameCore.ChangeScene<SceneTest>();

            if (KeyboardInputManager.IsKeyPressed(Keys.D1))
                GameCore.ChangeScene<SpriteAnimationScene>();

            if (KeyboardInputManager.IsKeyPressed(Keys.D2))
                GameCore.ChangeScene<SceneGraphScene>();

            if (KeyboardInputManager.IsKeyPressed(Keys.D3))
                GameCore.ChangeScene<CameraScene>();

            if (KeyboardInputManager.IsKeyPressed(Keys.D4))
                GameCore.ChangeScene<PhysicScene>();

            if (KeyboardInputManager.IsKeyPressed(Keys.D5))
                GameCore.ChangeScene<PlatformerTiledMapScene>();

            if (KeyboardInputManager.IsKeyPressed(Keys.D6))
                GameCore.ChangeScene<AetherPhysics2DHelloWorldScene>();
            
            if (KeyboardInputManager.IsKeyPressed(Keys.D7))
                GameCore.ChangeScene<QuadtreeCheckCollisionScene>();
            
            if (KeyboardInputManager.IsKeyPressed(Keys.D8))
                GameCore.ChangeScene<TiledMapWithManyLayersScene>();

            KeyboardInputManager.End();

            base.Update(gameTime);
        }
    }
}
