using MonoGame.Helper.ECS.Systems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Helper.ECS
{
    public sealed class SystemManager : IDisposable
    {
        readonly List<IInitializable> _initializableSystems = new List<IInitializable>();
        readonly List<IUpdatable> _updatableSystems = new List<IUpdatable>();
        readonly List<IRenderable> _renderableSystems = new List<IRenderable>();
        static readonly Lazy<SystemManager> _systemManager = new Lazy<SystemManager>(() => new SystemManager());

        SystemManager() { }

        public static SystemManager Instance => _systemManager.Value;

        public void AddSystem<T>(Scene scene, T system) where T : System
        {
            if (system is IInitializable && !_initializableSystems.Any(_ => _.GetType().Name == typeof(T).Name))
                _initializableSystems.Add(system as IInitializable);

            if (system is IUpdatable && !_updatableSystems.Any(_ => _.GetType().Name == typeof(T).Name))
                _updatableSystems.Add(system as IUpdatable);

            if (system is IRenderable && !_renderableSystems.Any(_ => _.GetType().Name == typeof(T).Name))
                _renderableSystems.Add(system as IRenderable);

            system.SetScene(scene);
        }

        public void AddSystem<T>(Scene scene, params object[] args) where T : System
        {
            var system = (T)Activator.CreateInstance(typeof(T), args);
            AddSystem(scene, system);
        }

        public void RemoveSystem<T>() where T : System
        {
            _initializableSystems.RemoveAll(_ => _.GetType().Name == typeof(T).Name);
            _updatableSystems.RemoveAll(_ => _.GetType().Name == typeof(T).Name);
            _renderableSystems.RemoveAll(_ => _.GetType().Name == typeof(T).Name);
        }

        public void InitializableSystemsIteration()
        {
            for (int i = 0; i < _initializableSystems.Count; i++)
                _initializableSystems[i].Initialize();
        }

        public void UpdatableSystemsIteration()
        {
            for (int i = 0; i < _updatableSystems.Count; i++)
                _updatableSystems[i].Update();
        }

        public void RenderableSystemsIteration()
        {
            for (int i = 0; i < _renderableSystems.Count; i++)
                _renderableSystems[i].Draw();
        }

        public void Dispose()
        {
            _initializableSystems.Clear();
            _updatableSystems.Clear();
            _renderableSystems.Clear();
            _systemManager.Value.Dispose();

            GC.Collect();
        }
    }
}
