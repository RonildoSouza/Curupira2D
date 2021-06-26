using Curupira2D.ECS;
using Curupira2D.GameComponents;
using Curupira2D.GameComponents.Camera2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Reflection;

namespace Curupira2D
{
    public abstract class GameCore : Game
    {
        readonly GraphicsDeviceManager _graphics;
        readonly FPSCounterComponent _fpsCounterComponent;
        readonly SceneManager _sceneManager = new SceneManager();
        readonly int _width;
        readonly int _height;
        readonly bool _disabledExit;

        public GameCore(int width = 800, int height = 480, bool debugActive = false, bool disabledExit = false)
        {
            _width = width;
            _height = height;
            DebugActive = debugActive;
            _disabledExit = disabledExit;

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            if (DebugActive)
            {
                _fpsCounterComponent = new FPSCounterComponent(this);
                Components.Add(_fpsCounterComponent);
            }
        }

        public int FPS => (_fpsCounterComponent?.FPS).GetValueOrDefault();

        internal bool DebugActive { get; }
        internal ICamera2D Camera2D { get; private set; }
        internal ICamera2D UICamera2D { get; private set; }

        protected override void Initialize()
        {
            _graphics.GraphicsProfile = GraphicsProfile.Reach;
            _graphics.PreferredBackBufferWidth = _width;
            _graphics.PreferredBackBufferHeight = _height;
            _graphics.ApplyChanges();

            Camera2D = new Camera2DComponent(this);
            Components.Add(Camera2D);

            UICamera2D = new Camera2DComponent(this);
            Components.Add(UICamera2D);

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!_disabledExit && (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)))
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
                Window.Title = $"{_sceneManager.CurrentScene?.Title ?? GetType().Assembly.GetName().Name} " +
                           $"| {GraphicsDevice.Viewport.Width}x{GraphicsDevice.Viewport.Height} " +
                           $"| FPS: {_fpsCounterComponent.FPS}" +
                           $"| v{GetVersion()}";
            }
            else
                Window.Title = $"{_sceneManager.CurrentScene?.Title ?? GetType().Assembly.GetName().Name}";

            base.Draw(gameTime);
        }

        public Scene SetScene(Scene scene) => _sceneManager.Set(this, scene);

        public TScene SetScene<TScene>(params object[] args) where TScene : Scene => _sceneManager.Set<TScene>(this, args);

        public void AddScene(Scene scene) => _sceneManager.Add(scene);

        public void AddScene<TScene>(params object[] args) where TScene : Scene => _sceneManager.Add<TScene>(args);

        public void ChangeScene<TScene>() where TScene : Scene => _sceneManager.Change<TScene>(this);

        public bool CurrentSceneIs<TScene>() where TScene : Scene => _sceneManager.CurrentScene.GetType() == typeof(TScene);

        public TScene GetCurrentScene<TScene>() where TScene : Scene => _sceneManager.CurrentScene as TScene;

        public Scene GetCurrentScene() => GetCurrentScene<Scene>();

        public string GetVersion()
        {
            var assembly = _sceneManager.CurrentScene != null ? _sceneManager.CurrentScene.GetType().Assembly : Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            if (_sceneManager.CurrentScene == null)
                return $"{GetType().Assembly.GetName().Name} Version - {fileVersionInfo.ProductVersion}";

            return fileVersionInfo.ProductVersion;
        }
    }
}
