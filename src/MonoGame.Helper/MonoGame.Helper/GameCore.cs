using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.ECS;
using MonoGame.Helper.GameComponents;
using System;
using System.Reflection;

namespace MonoGame.Helper
{
    public abstract class GameCore : Game
    {
        readonly GraphicsDeviceManager _graphics;
        readonly FPSCounterComponent _fpsCounterComponent;

        public static Scene CurrentScene { get; private set; }

        public GameCore(int width = 800, int height = 480)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;

#if DEBUG
            _fpsCounterComponent = new FPSCounterComponent(this);
            Components.Add(_fpsCounterComponent);
#endif
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CurrentScene?.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(CurrentScene?.CleanColor ?? Color.LightGray);

            CurrentScene?.Draw();

#if DEBUG
            Window.Title = $"{CurrentScene?.Title ?? GetType().GetTypeInfo().Assembly.GetName().Name} | {GraphicsDevice.Viewport.Width}x{GraphicsDevice.Viewport.Height} | FPS: {_fpsCounterComponent.FPS}";
#else
            Window.Title = $"{CurrentScene?.Title}";
#endif

            base.Draw(gameTime);
        }

        public void SetScene(Scene scene)
        {
            CurrentScene = scene;
            CurrentScene.SetGameCore(this);
            CurrentScene.Initialize();
        }

        public void SetScene<T>(params object[] args) where T : Scene
        {
            var scene = (T)Activator.CreateInstance(typeof(T), args);
            SetScene(scene);
        }
    }
}
