using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.ECS
{
    internal sealed class SystemManager : IDisposable
    {
        readonly List<ILoadable> _loadableSystems = new List<ILoadable>();
        readonly List<IUpdatable> _updatableSystems = new List<IUpdatable>();
        readonly List<IRenderable> _renderableSystems = new List<IRenderable>();

        public void LoadableSystemsIteration()
        {
            for (int i = 0; i < _loadableSystems.Count; i++)
                _loadableSystems[i].LoadContent();
        }

        public void UpdatableSystemsIteration()
        {
            for (int i = 0; i < _updatableSystems.Count; i++)
            {
                if (SystemIsValid(_updatableSystems[i]))
                    _updatableSystems[i].Update();
            }
        }

        public void RenderableSystemsIteration(Func<IRenderable, bool> predicate)
        {
            var renderableSystems = _renderableSystems.Where(predicate).ToList();

            for (int i = 0; i < renderableSystems.Count; i++)
            {
                if (SystemIsValid(renderableSystems[i]))
                {
                    var entities = renderableSystems[i].Scene.GetEntities(_ => renderableSystems[i].MatchActiveEntitiesAndComponents(_) && _.HasComponent(_ => _.Value is DrawableComponent));
                    renderableSystems[i].Draw(ref entities);
                }
            }
        }

        public void Add<TSystem>(Scene scene, TSystem system) where TSystem : System
        {
            if (system is ILoadable && !_loadableSystems.Any(_ => _.GetType().Name == typeof(TSystem).Name))
                _loadableSystems.Add(system as ILoadable);

            if (system is IUpdatable && !_updatableSystems.Any(_ => _.GetType().Name == typeof(TSystem).Name))
                _updatableSystems.Add(system as IUpdatable);

            if (system is IRenderable && !_renderableSystems.Any(_ => _.GetType().Name == typeof(TSystem).Name))
                _renderableSystems.Add(system as IRenderable);

            system.SetScene(scene);
        }

        public void Add<TSystem>(Scene scene, params object[] args) where TSystem : System
        {
            var system = (TSystem)Activator.CreateInstance(typeof(TSystem), args);
            Add(scene, system);
        }

        public void Remove<TSystem>() where TSystem : System
        {
            _loadableSystems.RemoveAll(Predicate);
            _updatableSystems.RemoveAll(Predicate);
            _renderableSystems.RemoveAll(Predicate);

            static bool Predicate(ISystem _)
            {
                var canRemove = _.GetType().Name == typeof(TSystem).Name;

                if (canRemove)
                    _.OnRemoveFromScene();

                return canRemove;
            }
        }

        public void RemoveAll()
        {
            _loadableSystems.RemoveAll(Predicate);
            _updatableSystems.RemoveAll(Predicate);
            _renderableSystems.RemoveAll(Predicate);

            static bool Predicate(ISystem _)
            {
                _.OnRemoveFromScene();
                return true;
            }
        }

        public void Dispose()
        {
            RemoveAll();
            GC.Collect();
        }

        bool SystemIsValid(ISystem system) => system.Scene != null && system.Scene.GameTime != null;
    }
}
