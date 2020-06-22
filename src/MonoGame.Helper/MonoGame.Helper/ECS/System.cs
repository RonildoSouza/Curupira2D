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

        public void SetScene(Scene scene)
        {
            this.AssertRequiredComponents(_requiredComponents);
            Scene = scene;
        }

        protected void AddRequiredComponent<T>() where T : IComponent
            => AddRequiredComponent(typeof(T));

        protected bool Matches(Entity entity)
        {
            this.AssertRequiredComponents(_requiredComponents);
            return entity.Active && entity.HasAllComponentTypes(_requiredComponents);
        }

        protected virtual void SetupRequiredComponents()
        {
            var requiredComponentAttrs = GetType().GetTypeInfo().GetCustomAttributes<RequiredComponentAttribute>();

            foreach (var rca in requiredComponentAttrs)
                AddRequiredComponent(rca.ComponentType);
        }

        void AddRequiredComponent(Type type)
        {
            if (_requiredComponents.Contains(type) || !type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IComponent)))
                return;

            _requiredComponents.Add(type);
        }
    }
}
