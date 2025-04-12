using Curupira2D.Desktop.Samples.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Desktop.Samples
{
    public class Game1 : GameCore
    {
        public Game1() : base(width: 800, height: 640, debugOptions: new Diagnostics.DebugOptions { DebugActive = true }) { }

        protected override void LoadContent()
        {
            SetScene<BehaviorTreeAndPathfinderScene>();
            //SetScene<MenuScene>();
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();

            if ((keyState.IsKeyDown(Keys.Q) || keyState.IsKeyDown(Keys.Back)) && !CurrentSceneIs<MenuScene>())
                SetScene<MenuScene>();

            base.Update(gameTime);
        }
    }
}
