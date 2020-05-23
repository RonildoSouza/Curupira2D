using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS.Systems;
using MonoGame.Helper.ECS.Systems.Drawable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Helper.ECS
{
    public class Scene : IDisposable
    {
        readonly EntityManager _entityManager = new EntityManager();
        readonly List<IInitializable> _initializableSystems = new List<IInitializable>();
        readonly List<IUpdatable> _updatableSystems = new List<IUpdatable>();
        readonly List<IRenderable> _renderableSystems = new List<IRenderable>();

        public GameCore GameCore { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public GameTime GameTime { get; private set; }
        public string Title { get; private set; }
        public Color CleanColor { get; private set; } = Color.LightGray;
        public float DeltaTime => (float)GameTime.ElapsedGameTime.TotalSeconds;
        public int ScreenWidth => GameCore.GraphicsDevice.Viewport.Width;
        public int ScreenHeight => GameCore.GraphicsDevice.Viewport.Height;

        public void SetGameCore(GameCore gameCore)
        {
            GameCore = gameCore;
            SpriteBatch = new SpriteBatch(GameCore.GraphicsDevice);
        }

        public Scene AddSystem<T>(T system) where T : System
        {
            if (system is IInitializable && !_initializableSystems.Any(_ => _.GetType().Name == typeof(T).Name))
                _initializableSystems.Add(system as IInitializable);

            if (system is IUpdatable && !_updatableSystems.Any(_ => _.GetType().Name == typeof(T).Name))
                _updatableSystems.Add(system as IUpdatable);

            if (system is IRenderable && !_renderableSystems.Any(_ => _.GetType().Name == typeof(T).Name))
                _renderableSystems.Add(system as IRenderable);

            system.SetScene(this);

            return this;
        }

        public Scene AddSystem<T>() where T : System
        {
            var system = (T)Activator.CreateInstance(typeof(T));
            return AddSystem(system);
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

        public void RemoveSystem<T>() where T : System
        {
            _initializableSystems.RemoveAll(_ => _.GetType().Name == typeof(T).Name);
            _updatableSystems.RemoveAll(_ => _.GetType().Name == typeof(T).Name);
            _renderableSystems.RemoveAll(_ => _.GetType().Name == typeof(T).Name);
        }

        public Entity CreateEntity(string uniqueId) => _entityManager.CreateEntity(uniqueId);

        public Entity GetEntity(string uniqueId) => _entityManager.GetEntity(uniqueId);

        public List<Entity> GetEntities(Func<Entity, bool> match) => _entityManager.GetEntities(match);

        public void DestroyEntity(Predicate<Entity> match) => _entityManager.DestroyEntity(match);

        public void DestroyEntity(string uniqueId) => _entityManager.DestroyEntity(uniqueId);

        public void ChangeGameScene(Scene scene)
        {
            GameCore.SetScene(scene);
        }

        public void ChangeGameScene<T>(params object[] args) where T : Scene
        {
            GameCore.SetScene<T>(args);
        }

        public virtual void Initialize()
        {
            AddSystem<SpriteSystem>();
            AddSystem<SpriteAnimationSystem>();

            for (int i = 0; i < _initializableSystems.Count; i++)
                _initializableSystems[i].Initialize();
        }

        public virtual void Update(GameTime gameTime)
        {
            GameTime = gameTime;

            for (int i = 0; i < _updatableSystems.Count; i++)
                _updatableSystems[i].Update();
        }

        public virtual void Draw()
        {
            SpriteBatch.Begin();

            for (int i = 0; i < _renderableSystems.Count; i++)
                _renderableSystems[i].Draw();

            SpriteBatch.End();
        }

        public void Dispose()
        {
            GameCore.Dispose();
            SpriteBatch.Dispose();
        }
    }
}
