using Curupira2D.Mobile.Samples.Scenes;

namespace Curupira2D.Mobile.Samples
{
    public class Game1 : GameCore
    {
        public Game1() : base(debugActive: true)
        {
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            SetScene<JoystickScene>();
            base.LoadContent();
        }
    }
}
