﻿using Curupira2D.Mobile.Samples.Scenes;

namespace Curupira2D.Mobile.Samples
{
    public class Game1 : GameCore
    {
        public Game1() : base(debugActive: false)
        {
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            AddScene<MenuScene>();
            AddScene<S01JoystickScene>();
            AddScene<S02TopDownCarMovementScene>();
            AddScene<S03AsteroidsMovementScene>();

            ChangeScene<MenuScene>();

            //SetScene<S02TopDownCarMovementScene>();

            base.LoadContent();
        }
    }
}