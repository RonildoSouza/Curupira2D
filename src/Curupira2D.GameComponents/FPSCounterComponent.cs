using Microsoft.Xna.Framework;
using System;

namespace Curupira2D.GameComponents
{
    public class FPSCounterComponent : DrawableGameComponent
    {
        TimeSpan _elapsedTime;
        int _totalFrames;

        public FPSCounterComponent(Game game) : base(game) { }

        public int FPS { get; private set; }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _totalFrames++;

            if (_elapsedTime >= TimeSpan.FromSeconds(1))
            {
                FPS = _totalFrames;
                _totalFrames = 0;
                _elapsedTime -= TimeSpan.FromSeconds(1);
            }

            base.Draw(gameTime);
        }
    }
}
