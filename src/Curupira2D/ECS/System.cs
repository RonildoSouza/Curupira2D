using Curupira2D.Diagnostics;
using Curupira2D.ECS.Components;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Curupira2D.ECS
{
    public interface ISystem : IDisposable
    {
        Scene Scene { get; }
        bool MatchActiveEntitiesAndComponents(Entity entity);
        void OnRemoveFromScene();
    }

    public abstract class System : ISystem
    {
        private readonly List<Type> _requiredComponents = [];

        public System()
        {
            SetupRequiredComponents();
        }

        ~System() => Dispose(disposing: false);

        public Scene Scene { get; private set; }

        public virtual void SetScene(Scene scene)
        {
            AssertRequiredComponents(_requiredComponents);
            Scene = scene;
        }

        protected void AddRequiredComponent<TComponent>() where TComponent : IComponent => AddRequiredComponent([typeof(TComponent)]);

        protected bool MatchComponents(Entity entity)
        {
            AssertRequiredComponents(_requiredComponents);
            return entity.HasAllComponentTypes(_requiredComponents);
        }

        public bool MatchActiveEntitiesAndComponents(Entity entity) => entity.Active && MatchComponents(entity);

        public virtual void OnRemoveFromScene() { }

        protected virtual void SetupRequiredComponents()
        {
            var requiredComponentAttrs = GetType().GetTypeInfo().GetCustomAttributes<RequiredComponentAttribute>();

            for (int i = 0; i < requiredComponentAttrs.Count(); i++)
                AddRequiredComponent(requiredComponentAttrs.ElementAt(i).ComponentTypes);
        }

        private void AddRequiredComponent(Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];

                if (_requiredComponents.Contains(type) || !type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IComponent)))
                    continue;

                _requiredComponents.Add(type);
            }
        }

        private void AssertRequiredComponents(List<Type> requiredComponentTypes)
        {
            var implementOnlyILoadable = GetType().GetTypeInfo().ImplementedInterfaces.Count() == 3
                && GetType().GetTypeInfo().ImplementedInterfaces.Contains(typeof(ISystem))
                && GetType().GetTypeInfo().ImplementedInterfaces.Contains(typeof(ILoadable));

            if (requiredComponentTypes.Count == 0 && GetType().Name != nameof(DebugSystem) && !implementOnlyILoadable)
                throw new Exception($"You should add required component for the system {GetType().Name}");
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) { }
    }
}
