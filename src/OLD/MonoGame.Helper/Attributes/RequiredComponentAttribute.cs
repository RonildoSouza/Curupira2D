using MonoGame.Helper.ECS.Components;
using System;
using System.Linq;
using System.Reflection;

namespace MonoGame.Helper.Attributes
{
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
