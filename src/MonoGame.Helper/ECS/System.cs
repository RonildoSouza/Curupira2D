using MonoGame.Helper.Diagnostics;
using MonoGame.Helper.ECS.Components;
using MonoGame.Helper.ECS.Systems;
using MonoGame.Helper.ECS.Systems.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MonoGame.Helper.ECS
{
    internal interface ISystem { }

    public abstract class System : ISystem
    {
        readonly List<Type> _requiredComponents = new List<Type>();

        public System()
        {
            SetupRequiredComponents();
        }

        protected Scene Scene { get; private set; }

        public virtual void SetScene(Scene scene)
        {
            AssertRequiredComponents(_requiredComponents);
            Scene = scene;
        }

        protected void AddRequiredComponent<TComponent>() where TComponent : IComponent
            => AddRequiredComponent(new Type[] { typeof(TComponent) });

        protected bool MatchComponents(Entity entity)
        {
            AssertRequiredComponents(_requiredComponents);
            return entity.HasAllComponentTypes(_requiredComponents);
        }

        protected bool MatchActiveEntitiesAndComponents(Entity entity)
            => entity.Active && MatchComponents(entity);

        protected virtual void SetupRequiredComponents()
        {
            var requiredComponentAttrs = GetType().GetTypeInfo().GetCustomAttributes<RequiredComponentAttribute>();

            foreach (var requiredComponentAttr in requiredComponentAttrs)
                AddRequiredComponent(requiredComponentAttr.ComponentTypes);
        }

        void AddRequiredComponent(Type[] types)
        {
            foreach (var type in types)
            {
                if (_requiredComponents.Contains(type) || !type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IComponent)))
                    continue;

                _requiredComponents.Add(type);
            }
        }

        void AssertRequiredComponents(List<Type> requiredComponentTypes)
        {
            var implementOnlyIInitializable = GetType().GetTypeInfo().ImplementedInterfaces.Count() == 2
                && GetType().GetTypeInfo().ImplementedInterfaces.Contains(typeof(ISystem))
                && GetType().GetTypeInfo().ImplementedInterfaces.Contains(typeof(IInitializable));

            if (!requiredComponentTypes.Any() && GetType().Name != nameof(DebugSystem) && !implementOnlyIInitializable)
                throw new Exception($"You should add required component for the system {GetType().Name}");
        }
    }
}
