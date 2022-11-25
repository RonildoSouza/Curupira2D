using Curupira2D.Mobile.Samples.Scenes;

namespace Curupira2D.Mobile.Samples
{
    public class Game1 : GameCore
    {
        public Game1() : base()
        {
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            SetScene<MenuScene>();

            base.LoadContent();
        }
    }
}
