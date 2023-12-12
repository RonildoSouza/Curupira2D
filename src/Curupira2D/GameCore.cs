using Curupira2D.Diagnostics;
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
        readonly bool _isFullScreen;

        public GameCore(int width = 0, int height = 0, bool disabledExit = false, DebugOptions debugOptions = null, bool isFullScreen = false)
        {
            DebugOptions = debugOptions ?? new DebugOptions();

            _width = width;
            _height = height;
            _disabledExit = disabledExit;
            _isFullScreen = isFullScreen;

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            if (DebugOptions.DebugActive)
            {
                _fpsCounterComponent = new FPSCounterComponent(this);
                Components.Add(_fpsCounterComponent);
                Components.Add(new DebugComponent(this));
            }
        }

        public int FPS => (_fpsCounterComponent?.FPS).GetValueOrDefault();
        public GraphicsDeviceManager GraphicsDeviceManager => _graphics;

        internal DebugOptions DebugOptions { get; }
        internal ICamera2D Camera2D { get; private set; }
        internal ICamera2D UICamera2D { get; private set; }

        protected override void Initialize()
        {
            _graphics.GraphicsProfile = GraphicsProfile.Reach;

            if (_width != 0 && _height != 0)
            {
                _graphics.PreferredBackBufferWidth = _width;
                _graphics.PreferredBackBufferHeight = _height;
            }

            if (_isFullScreen)
            {
                _graphics.IsFullScreen = _isFullScreen;
                _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            }

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

            if (!DebugOptions.DebugActive)
                Window.Title = !string.IsNullOrEmpty(_sceneManager.CurrentScene?.Title) ? _sceneManager.CurrentScene.Title : GetType().Assembly.GetName().Name;

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            _sceneManager.CurrentScene?.UnloadContent();
            base.UnloadContent();
        }

        public Scene SetScene(Scene scene) => _sceneManager.Set(this, scene);

        public TScene SetScene<TScene>(params object[] args) where TScene : Scene => _sceneManager.Set<TScene>(this, args);

        public bool CurrentSceneIs<TScene>() where TScene : Scene => _sceneManager.CurrentScene.GetType() == typeof(TScene);

        public TScene GetCurrentScene<TScene>() where TScene : Scene => _sceneManager.CurrentScene as TScene;

        public Scene GetCurrentScene() => GetCurrentScene<Scene>();

        public string GetVersion()
        {
            var assembly = _sceneManager.CurrentScene != null ? _sceneManager.CurrentScene.GetType().Assembly : Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            if (_sceneManager.CurrentScene == null)
                return $"{GetType().Assembly.GetName().Name} Version - {fileVersionInfo.FileVersion}";

            return fileVersionInfo.FileVersion;
        }
    }
}
