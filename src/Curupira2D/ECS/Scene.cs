using Curupira2D.ECS.Systems.Drawables;
using Curupira2D.ECS.Systems.Physics;
using Curupira2D.GameComponents.Camera2D;
using Curupira2D.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.ECS
{
    public class Scene : IDisposable
    {
        readonly EntityManager _entityManager = EntityManager.Instance;
        readonly SystemManager _systemManager = SystemManager.Instance;

        public GameCore GameCore { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public GameTime GameTime { get; private set; }
        public ICamera2D Camera2D { get; private set; }
        public ICamera2D UICamera2D { get; private set; }
        public string Title { get; private set; }
        public Color CleanColor { get; private set; } = Color.LightGray;
        public float DeltaTime { get; private set; }
        public int ScreenWidth => GameCore.GraphicsDevice.Viewport.Width;
        public int ScreenHeight => GameCore.GraphicsDevice.Viewport.Height;
        public Vector2 ScreenSize => new Vector2(ScreenWidth, ScreenHeight);
        public Vector2 ScreenCenter => new Vector2(ScreenWidth * 0.5f, ScreenHeight * 0.5f);

        public KeyboardInputManager KeyboardInputManager { get; private set; }
        public GamePadInputManager GamePadInputManager { get; private set; }
        public MouseInputManager MouseInputManager { get; private set; }

        public Vector2 Gravity { get; set; }

        public void SetGameCore(GameCore gameCore)
        {
            GameCore = gameCore;
            SpriteBatch = new SpriteBatch(GameCore.GraphicsDevice);

            var cameraInitialOrigin = new Vector2(ScreenWidth * 0.5f, ScreenHeight * 0.5f);
            var cameraInitialPosition = new Vector2(ScreenWidth * 0.5f, ScreenHeight * 0.5f);

            GameCore.Camera2D.Reset();
            GameCore.Camera2D.Origin = cameraInitialOrigin;
            GameCore.Camera2D.Position = cameraInitialPosition;
            Camera2D = GameCore.Camera2D;

            GameCore.UICamera2D.Reset();
            GameCore.UICamera2D.Origin = cameraInitialOrigin;
            GameCore.UICamera2D.Position = cameraInitialPosition;
            UICamera2D = GameCore.UICamera2D;
        }

        public virtual void LoadContent()
        {
            AddSystem<SpriteSystem>();
            AddSystem<SpriteAnimationSystem>();
            AddSystem<TiledMapSystem>();
            AddSystem<TextSystem>();

            // Always keep this system at the end
            AddSystem(new PhysicsSystem(Gravity));

            _systemManager.LoadableSystemsIteration();

            KeyboardInputManager = new KeyboardInputManager();
            GamePadInputManager = new GamePadInputManager();
            MouseInputManager = new MouseInputManager();
        }

        public virtual void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardInputManager.Begin();
            GamePadInputManager.Begin();
            MouseInputManager.Begin();

            _systemManager.UpdatableSystemsIteration();

            KeyboardInputManager.End();
            GamePadInputManager.End();
            MouseInputManager.End();
        }

        public virtual void Draw()
        {
            _systemManager.RenderableSystemsIteration();
        }

        public Scene SetTitle(string title)
        {
            Title = title;
            return this;
        }

        public Scene SetCleanColor(Color cleanColor)
        {
            CleanColor = cleanColor;
            return this;
        }

        public float InvertPositionX(float x) => ScreenWidth - x;

        public float InvertPositionY(float y) => ScreenHeight - y;

        #region Methods of managing game core
        public Scene AddGameComponent(IGameComponent gameComponent)
        {
            if (!GameCore.Components.Any(c => c.GetType() == gameComponent.GetType()))
                GameCore.Components.Add(gameComponent);

            return this;
        }

        public Scene RemoveGameComponent(IGameComponent gameComponent)
        {
            if (GameCore.Components.Any(c => c.GetType() == gameComponent.GetType()))
            {
                var index = GameCore.Components.ToList().FindIndex(c => c.GetType() == gameComponent.GetType());
                GameCore.Components.RemoveAt(index);
            }

            return this;
        }
        #endregion

        #region Methods of managing systems
        public Scene AddSystem<TSystem>(TSystem system) where TSystem : System
        {
            _systemManager.Add(this, system);
            return this;
        }

        public Scene AddSystem<TSystem>(params object[] args) where TSystem : System
        {
            _systemManager.Add<TSystem>(this, args);
            return this;
        }

        public void RemoveSystem<TSystem>() where TSystem : System => _systemManager.Remove<TSystem>();

        public void RemoveAllSystems() => _systemManager.RemoveAll();
        #endregion

        #region Methods of managing entities
        public Entity CreateEntity(string uniqueId, string group = null) => _entityManager.Create(uniqueId, group);

        public Entity GetEntity(string uniqueId) => _entityManager.Get(uniqueId);

        public IReadOnlyList<Entity> GetEntities(Func<Entity, bool> match) => _entityManager.GetAll(match);

        public IReadOnlyList<Entity> GetEntities(string group) => GetEntities(e => e.Group == group);

        public void RemoveEntity(Predicate<Entity> match) => _entityManager.Remove(match);

        public void RemoveEntity(string uniqueId) => _entityManager.Remove(uniqueId);

        public void RemoveEntity(Entity entity) => RemoveEntity(entity?.UniqueId);

        public void RemoveAllEntities() => _entityManager.RemoveAll();

        public bool ExistsEntities(Func<Entity, bool> match) => _entityManager.Exists(match);
        #endregion       

        public virtual void Dispose()
        {
            GameCore.Dispose();
            SpriteBatch.Dispose();
            Camera2D = null;
            UICamera2D = null;

            GC.Collect();
        }
    }
}
