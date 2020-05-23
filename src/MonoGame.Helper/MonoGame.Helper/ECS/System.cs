using MonoGame.Helper.Diagnostics;
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
            AssertRequiredComponents();
            Scene = scene;
        }

        protected void AddRequiredComponent<T>() where T : IComponent
            => AddRequiredComponent(typeof(T));

        protected bool Matches(Entity entity)
        {
            AssertRequiredComponents();
            return entity.Transform.Active && entity.HasAllComponentTypes(_requiredComponents);
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

        void AssertRequiredComponents()
        {
            if (!_requiredComponents.Any() && GetType().Name != nameof(DebugSystem))
                throw new Exception($"You should add required component for the system {GetType().Name}");
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredComponentAttribute : Attribute
    {
        public RequiredComponentAttribute(Type componentType)
        {
            if (!(componentType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IComponent))))
                throw new ArgumentException($"The argument {nameof(componentType)} is not a {nameof(IComponent)}");

            ComponentType = componentType;
        }

        public Type ComponentType { get; }
    }
}
