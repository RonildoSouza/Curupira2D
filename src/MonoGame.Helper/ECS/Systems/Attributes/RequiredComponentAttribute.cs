using MonoGame.Helper.ECS.Components;
using System;
using System.Linq;
using System.Reflection;

namespace MonoGame.Helper.ECS.Systems.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredComponentAttribute : Attribute
    {
        public RequiredComponentAttribute(Type system, Type[] componentTypes)
        {
            if (!system.GetTypeInfo().ImplementedInterfaces.Contains(typeof(ISystem)))
                throw new ArgumentException($"{nameof(RequiredComponentAttribute)} should be use with {nameof(System)} type classes");

            if (componentTypes == null || !componentTypes.Any())
                throw new ArgumentException($"The argument {nameof(componentTypes)} is required!");

            if (!componentTypes.All(c => c.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IComponent))))
                throw new ArgumentException($"One or more is not a {nameof(IComponent)}");

            ComponentTypes = componentTypes;
        }

        public RequiredComponentAttribute(Type system, Type componentType) : this(system, new Type[] { componentType }) { }

        public Type[] ComponentTypes { get; }
    }
}
