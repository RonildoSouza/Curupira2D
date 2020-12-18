using MonoGame.Helper.Asserts;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MonoGame.Helper.ECS
{
    public abstract class System
    {
        readonly List<Type> _requiredComponents = new List<Type>();

        public System()
        {
            SetupRequiredComponents();
        }

        protected Scene Scene { get; private set; }

        public virtual void SetScene(Scene scene)
        {
            this.AssertRequiredComponents(_requiredComponents);
            Scene = scene;
        }

        protected void AddRequiredComponent<TComponent>() where TComponent : IComponent
            => AddRequiredComponent(typeof(TComponent));

        protected bool MatchComponents(Entity entity)
        {
            this.AssertRequiredComponents(_requiredComponents);
            return entity.HasAllComponentTypes(_requiredComponents);
        }

        protected bool MatchActiveEntitiesAndComponents(Entity entity)
            => entity.Active && MatchComponents(entity);

        protected virtual void SetupRequiredComponents()
        {
            var requiredComponentAttrs = GetType().GetTypeInfo().GetCustomAttributes<RequiredComponentAttribute>();

            foreach (var requiredComponentAttr in requiredComponentAttrs)
                AddRequiredComponent(requiredComponentAttr.ComponentType);
        }

        void AddRequiredComponent(Type type)
        {
            if (_requiredComponents.Contains(type) || !type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IComponent)))
                return;

            _requiredComponents.Add(type);
        }
    }
}
