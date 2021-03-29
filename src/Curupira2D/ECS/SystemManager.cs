using Curupira2D.ECS.Systems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.ECS
{
    internal sealed class SystemManager : IDisposable
    {
        readonly List<IInitializable> _initializableSystems = new List<IInitializable>();
        readonly List<IUpdatable> _updatableSystems = new List<IUpdatable>();
        readonly List<IRenderable> _renderableSystems = new List<IRenderable>();
        static readonly Lazy<SystemManager> _systemManager = new Lazy<SystemManager>(() => new SystemManager());

        SystemManager() { }

        public static SystemManager Instance => _systemManager.Value;

        public void AddSystem<TSystem>(Scene scene, TSystem system) where TSystem : System
        {
            if (system is IInitializable && !_initializableSystems.Any(_ => _.GetType().Name == typeof(TSystem).Name))
                _initializableSystems.Add(system as IInitializable);

            if (system is IUpdatable && !_updatableSystems.Any(_ => _.GetType().Name == typeof(TSystem).Name))
                _updatableSystems.Add(system as IUpdatable);

            if (system is IRenderable && !_renderableSystems.Any(_ => _.GetType().Name == typeof(TSystem).Name))
                _renderableSystems.Add(system as IRenderable);

            system.SetScene(scene);
        }

        public void AddSystem<TSystem>(Scene scene, params object[] args) where TSystem : System
        {
            var system = (TSystem)Activator.CreateInstance(typeof(TSystem), args);
            AddSystem(scene, system);
        }

        public void RemoveSystem<TSystem>() where TSystem : System
        {
            _initializableSystems.RemoveAll(_ => _.GetType().Name == typeof(TSystem).Name);
            _updatableSystems.RemoveAll(_ => _.GetType().Name == typeof(TSystem).Name);
            _renderableSystems.RemoveAll(_ => _.GetType().Name == typeof(TSystem).Name);
        }

        public void RemoveAllSystems()
        {
            _initializableSystems.RemoveAll(_ => true);
            _updatableSystems.RemoveAll(_ => true);
            _renderableSystems.RemoveAll(_ => true);
        }

        public void InitializableSystemsIteration()
        {
            for (int i = 0; i < _initializableSystems.Count; i++)
                _initializableSystems[i].Initialize();
        }

        public void UpdatableSystemsIteration()
        {
            for (int i = 0; i < _updatableSystems.Count; i++)
            {
                if (!SystemIsValid(_updatableSystems[i]))
                    continue;

                _updatableSystems[i].Update();
            }
        }

        public void RenderableSystemsIteration()
        {
            for (int i = 0; i < _renderableSystems.Count; i++)
            {
                if (!SystemIsValid(_renderableSystems[i]))
                    continue;

                _renderableSystems[i].Draw();
            }
        }

        public void Dispose()
        {
            _initializableSystems.Clear();
            _updatableSystems.Clear();
            _renderableSystems.Clear();
            _systemManager.Value.Dispose();

            GC.Collect();
        }

        private bool SystemIsValid(ISystem system)
            => system.Scene != null && system.Scene.GameTime != null;
    }
}
