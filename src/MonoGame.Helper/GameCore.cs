using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.ECS;
using MonoGame.Helper.GameComponents;
using System.Reflection;

namespace MonoGame.Helper
{
    public abstract class GameCore : Game
    {
        readonly GraphicsDeviceManager _graphics;
        readonly FPSCounterComponent _fpsCounterComponent;
        readonly SceneManager _sceneManager = SceneManager.Instance;

        internal bool DebugActive { get; }

        public GameCore(int width = 800, int height = 480, bool debugActive = false)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;

            DebugActive = debugActive;

            if (DebugActive)
            {
                _fpsCounterComponent = new FPSCounterComponent(this);
                Components.Add(_fpsCounterComponent);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _sceneManager.CurrentScene?.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_sceneManager.CurrentScene?.CleanColor ?? Color.LightGray);

            _sceneManager.CurrentScene?.Draw();

            if (DebugActive)
            {
                Window.Title = $"{_sceneManager.CurrentScene?.Title ?? GetType().GetTypeInfo().Assembly.GetName().Name} " +
                           $"| {GraphicsDevice.Viewport.Width}x{GraphicsDevice.Viewport.Height} " +
                           $"| FPS: {_fpsCounterComponent.FPS}";
            }
            else
                Window.Title = $"{_sceneManager.CurrentScene?.Title ?? GetType().GetTypeInfo().Assembly.GetName().Name}";

            base.Draw(gameTime);
        }

        public void SetScene(Scene scene) => _sceneManager.SetScene(this, scene);

        public void SetScene<TScene>(params object[] args) where TScene : Scene
             => _sceneManager.SetScene<TScene>(this, args);

        public void AddScene(Scene scene) => _sceneManager.AddScene(scene);

        public void ChangeScene<TScene>() where TScene : Scene
            => _sceneManager.ChangeScene<TScene>(this);
    }
}
