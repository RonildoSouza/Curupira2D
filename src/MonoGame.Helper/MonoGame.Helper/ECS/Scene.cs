using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS.Systems.Drawable;
using System;
using System.Collections.Generic;

namespace MonoGame.Helper.ECS
{
    public class Scene : IDisposable
    {
        readonly EntityManager _entityManager = EntityManager.Instance;
        readonly SystemManager _systemManager = SystemManager.Instance;

        public GameCore GameCore { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public GameTime GameTime { get; private set; }
        public string Title { get; private set; }
        public Color CleanColor { get; private set; } = Color.LightGray;
        public Vector2 Gravity { get; set; } = new Vector2(0f, 9.80665f);
        public float DeltaTime => (float)GameTime.ElapsedGameTime.TotalSeconds;
        public int ScreenWidth => GameCore.GraphicsDevice.Viewport.Width;
        public int ScreenHeight => GameCore.GraphicsDevice.Viewport.Height;

        public void SetGameCore(GameCore gameCore)
        {
            GameCore = gameCore;
            SpriteBatch = new SpriteBatch(GameCore.GraphicsDevice);
        }

        public Scene AddSystem<TSystem>(TSystem system) where TSystem : System
        {
            _systemManager.AddSystem(this, system);
            return this;
        }

        public Scene AddSystem<TSystem>(params object[] args) where TSystem : System
        {
            _systemManager.AddSystem<TSystem>(this, args);
            return this;
        }

        public void RemoveSystem<TSystem>() where TSystem : System
            => _systemManager.RemoveSystem<TSystem>();

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

        public Entity CreateEntity(string uniqueId) => _entityManager.CreateEntity(uniqueId);

        public Entity GetEntity(string uniqueId) => _entityManager.GetEntity(uniqueId);

        public IReadOnlyList<Entity> GetEntities(Func<Entity, bool> match) => _entityManager.GetEntities(match);

        public void DestroyEntity(Predicate<Entity> match) => _entityManager.DestroyEntity(match);

        public void DestroyEntity(string uniqueId) => _entityManager.DestroyEntity(uniqueId);

        public virtual void Initialize()
        {
            AddSystem<SpriteSystem>();
            AddSystem<SpriteAnimationSystem>();

            _systemManager.InitializableSystemsIteration();
        }

        public virtual void Update(GameTime gameTime)
        {
            GameTime = gameTime;

            _systemManager.UpdatableSystemsIteration();
        }

        public virtual void Draw()
        {
            SpriteBatch.Begin(SpriteSortMode.BackToFront);

            _systemManager.RenderableSystemsIteration();

            SpriteBatch.End();
        }

        public void Dispose()
        {
            GameCore.Dispose();
            SpriteBatch.Dispose();

            GC.Collect();
        }
    }
}
