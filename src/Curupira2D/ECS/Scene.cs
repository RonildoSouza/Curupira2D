using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems.Drawables;
using Curupira2D.ECS.Systems.Physics;
using Curupira2D.GameComponents.Camera2D;
using Curupira2D.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curupira2D.ECS
{
    public class Scene : IDisposable
    {
        readonly EntityManager _entityManager = new();
        readonly SystemManager _systemManager = new();
        PhysicsSystem _physicsSystem;
        float _deltaTime;
        readonly List<IGameComponent> _gameComponents = [];
        bool _disposed = false;

        ~Scene() => Dispose(disposing: false);

        public GameCore GameCore { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public GameTime GameTime { get; private set; }
        public ICamera2D Camera2D { get; private set; }
        public ICamera2D UICamera2D { get; private set; }
        public string Title { get; private set; }
        public Color FallbackCleanColor { get; private set; }
        public Color CleanColor { get; private set; }

        /// <summary>
        /// The time in seconds since the last update.
        /// </summary>
        public float DeltaTime { get => _deltaTime == 0 ? 1f / 60f : _deltaTime; private set => _deltaTime = value; }
        public int ScreenWidth => GameCore.GraphicsDevice.Viewport.Width;
        public int ScreenHeight => GameCore.GraphicsDevice.Viewport.Height;
        public Vector2 ScreenSize => new(ScreenWidth, ScreenHeight);
        public Vector2 ScreenCenter => new(ScreenWidth * 0.5f, ScreenHeight * 0.5f);
        public SpriteSortMode SpriteSortMode { get; set; } = SpriteSortMode.FrontToBack;
        public SamplerState SamplerState { get; set; } = SamplerState.PointClamp;

        public KeyboardInputManager KeyboardInputManager { get; private set; }
        public GamePadInputManager GamePadInputManager { get; private set; }
        public MouseInputManager MouseInputManager { get; private set; }

        public Quadtree Quadtree { get; private set; }

        public Vector2 Gravity { get; set; }

        public bool PauseUpdatableSystems { get; set; }

        public virtual void LoadContent()
        {
            AddSystem<TiledMapSystem>();
            AddSystem<SpriteSystem>();
            AddSystem<SpriteAnimationSystem>();
            AddSystem<TextSystem>();

            // Always keep this system at the end
            _physicsSystem = new PhysicsSystem();
            AddSystem(_physicsSystem);

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

            if (!PauseUpdatableSystems)
                _systemManager.UpdatableSystemsIteration();

            KeyboardInputManager.End();
            GamePadInputManager.End();
            MouseInputManager.End();
        }

        public virtual void Draw()
        {
            #region Begin/End sprite batch to Camera
            SpriteBatch.Begin(
                sortMode: SpriteSortMode,
                samplerState: SamplerState,
                transformMatrix: Camera2D.TransformationMatrix
                );

            _systemManager.RenderableSystemsIteration(system =>
            {
                return system.Scene.GetEntities(_ =>
                {
                    var drawableComponent = _.GetComponent(_ => _.Value is DrawableComponent) as DrawableComponent;
                    return system.MatchActiveEntitiesAndComponents(_) && (!drawableComponent?.DrawInUICamera ?? false);
                }).Count != 0;
            });

            SpriteBatch.End();
            #endregion

            #region Begin/End sprite batch to UI Camera
            SpriteBatch.Begin(
                sortMode: SpriteSortMode,
                samplerState: SamplerState,
                transformMatrix: UICamera2D.TransformationMatrix
                );

            _systemManager.RenderableSystemsIteration(system =>
            {
                return system.Scene.GetEntities(_ =>
                {
                    var drawableComponent = _.GetComponent(_ => _.Value is DrawableComponent) as DrawableComponent;
                    return system.MatchActiveEntitiesAndComponents(_) && (drawableComponent?.DrawInUICamera ?? false);
                }).Count != 0;
            });

            SpriteBatch.End();
            #endregion

            _physicsSystem.DrawDebugData();
        }

        public virtual void UnloadContent()
        {
            RemoveAllSystems();
            RemoveAllEntities();
            RemoveAllGameComponents();
        }

        public Scene SetTitle(string title)
        {
            Title = title;
            return this;
        }

        public Scene SetCleanColor(Color cleanColor)
        {
            if (CleanColor != cleanColor)
                FallbackCleanColor = CleanColor;

            CleanColor = cleanColor;
            return this;
        }

        public float InvertPositionX(float x) => ScreenWidth - x;

        public Vector2 InvertPositionX(Vector2 position) => new(InvertPositionX(position.X), position.Y);

        public float InvertPositionY(float y) => ScreenHeight - y;

        public Vector2 InvertPositionY(Vector2 position) => new(position.X, InvertPositionY(position.Y));

        public void SetFallbackCleanColor() => CleanColor = FallbackCleanColor;

        #region Methods of managing game core
        public Scene AddGameComponent(IGameComponent gameComponent)
        {
            if (!GameCore.Components.Any(c => c.Equals(gameComponent) || c.GetType() == gameComponent.GetType()))
            {
                _gameComponents.Add(gameComponent);
                GameCore.Components.Add(gameComponent);
            }

            return this;
        }

        public Scene RemoveGameComponent(IGameComponent gameComponent)
        {
            if (GameCore.Components.Any(c => c.Equals(gameComponent) || c.GetType() == gameComponent.GetType()))
            {
                var index = GameCore.Components.ToList().FindIndex(c => c.Equals(gameComponent) || c.GetType() == gameComponent.GetType());
                GameCore.Components.RemoveAt(index);
                _gameComponents.RemoveAll(c => c.Equals(gameComponent) || c.GetType() == gameComponent.GetType());
            }

            return this;
        }

        public void RemoveAllGameComponents()
        {
            for (int i = 0; i < _gameComponents.Count; i++)
                RemoveGameComponent(_gameComponents[i]);

            _gameComponents.Clear();
        }

        public TGameComponent GetGameComponent<TGameComponent>() where TGameComponent : IGameComponent
            => (TGameComponent)GameCore.Components.FirstOrDefault(_ => _.GetType() == typeof(TGameComponent));
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

        public TSystem GetSystem<TSystem>() where TSystem : System => _systemManager.Get<TSystem>();

        public void RemoveSystem<TSystem>() where TSystem : System => _systemManager.Remove<TSystem>();

        public void RemoveAllSystems() => _systemManager.RemoveAll();
        #endregion

        #region Methods of managing entities
        public Entity CreateEntity(string uniqueId, Vector2 position, string group = null, bool isCollidable = true)
        {
            var entity = _entityManager.Create(uniqueId, position, group, isCollidable);
            entity.OnChange += Entity_OnChange;

            return entity;
        }

        public Entity CreateEntity(string uniqueId, float x, float y, string group = null, bool isCollidable = true)
            => CreateEntity(uniqueId, new Vector2(x, y), group, isCollidable);

        public Entity GetEntity(string uniqueId) => _entityManager.Get(uniqueId);

        public IReadOnlyCollection<Entity> GetEntities(Func<Entity, bool> match) => _entityManager.GetAll(match);

        public IReadOnlyCollection<Entity> GetEntities(string group) => GetEntities(e => e.Group == group);

        public void RemoveEntity(Predicate<Entity> match)
        {
            _entityManager.Remove(match);

            var entities = GetEntities(new Func<Entity, bool>(match));
            Parallel.ForEach(entities, entity => Quadtree.Delete(entity));
        }

        public void RemoveEntity(string uniqueId)
        {
            _entityManager.Remove(uniqueId);
            Quadtree.Delete(GetEntity(uniqueId));
        }

        public void RemoveEntity(Entity entity) => RemoveEntity(entity?.UniqueId);

        public void RemoveAllEntities()
        {
            _entityManager.RemoveAll();
            Quadtree.Clear();
        }

        public bool ExistsEntities(Func<Entity, bool> match) => _entityManager.Exists(match);

        public bool ExistsEntities(string uniqueId) => ExistsEntities(e => e.UniqueId == uniqueId);
        #endregion

        public virtual void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _systemManager.Dispose();
                _entityManager.Dispose();
                GameCore.Dispose();
                SpriteBatch.Dispose();
                Camera2D.Dispose();
                UICamera2D.Dispose();
                Quadtree.Dispose();
            }

            _disposed = true;
        }

        internal void SetGameCore(GameCore gameCore)
        {
            var entities = _entityManager.GetAll(_ => true);

            for (var i = 0; i < entities.Count; i++)
                entities[i].OnChange -= Entity_OnChange;

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

            Gravity = new Vector2(0f, 9.80665f);
            CleanColor = FallbackCleanColor = Color.LightGray;

            Quadtree = new Quadtree(GameCore.GraphicsDevice.Viewport.Bounds);
        }

        void Entity_OnChange(object sender, EventArgs e)
        {
            if (sender is not Entity entity
                || !(entity?.Active ?? false)
                || !(entity?.IsCollidable ?? false))
                return;

            Quadtree.Delete(entity);
            Quadtree.Insert(entity);
        }
    }
}
