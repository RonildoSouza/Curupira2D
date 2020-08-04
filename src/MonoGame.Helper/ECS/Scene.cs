using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS.Systems.Drawables;
using MonoGame.Helper.ECS.Systems.Physics;
using System;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGame.Helper.ECS
{
    public class Scene : IDisposable
    {
        Vector2 _gravity;
        readonly EntityManager _entityManager = EntityManager.Instance;
        readonly SystemManager _systemManager = SystemManager.Instance;

        public Scene(Vector2 gravity = default)
        {
            if (gravity == default)
                SetGravity(new Vector2(0f, 9.80665f));
        }

        public GameCore GameCore { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public GameTime GameTime { get; private set; }
        public Camera Camera { get; private set; }
        public World World { get; private set; }
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

        public Scene SetGravity(Vector2 gravity)
        {
            if (World == null)
                _gravity = gravity;
            else
                World.Gravity = gravity;

            return this;
        }

        public Entity CreateEntity(string uniqueId) => _entityManager.CreateEntity(uniqueId);

        public Entity GetEntity(string uniqueId) => _entityManager.GetEntity(uniqueId);

        public IReadOnlyList<Entity> GetEntities(Func<Entity, bool> match) => _entityManager.GetEntities(match);

        public void DestroyEntity(Predicate<Entity> match) => _entityManager.DestroyEntity(match);

        public void DestroyEntity(string uniqueId) => _entityManager.DestroyEntity(uniqueId);

        public virtual void Initialize()
        {
            Camera = new Camera(GameCore.GraphicsDevice)
            {
                Position = new Vector2(ScreenWidth * 0.5f, ScreenHeight * 0.5f)
            };
            Camera.LoadContent();

            World = new World(_gravity);

            AddSystem<TextSystem>();
            AddSystem<SpriteSystem>();
            AddSystem<SpriteAnimationSystem>();
            AddSystem<TiledMapSystem>();

            // Always keep this system at the end
            AddSystem<PhysicsSystem>();

            _systemManager.InitializableSystemsIteration();
        }

        public virtual void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            Camera.Update(gameTime);
            World.Step(DeltaTime);
            _systemManager.UpdatableSystemsIteration();
        }

        public virtual void Draw()
        {
            SpriteBatch.Draw(Camera.Debug);
            _systemManager.RenderableSystemsIteration();
        }

        public virtual void Dispose()
        {
            GameCore.Dispose();
            SpriteBatch.Dispose();

            GC.Collect();
        }
    }
}
