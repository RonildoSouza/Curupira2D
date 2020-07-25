using MonoGame.Helper.Diagnostics;
using MonoGame.Helper.ECS.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MonoGame.Helper.Asserts
{
    public static class AssertSystem
    {
        public static void AssertRequiredComponents(this ECS.System system, List<Type> requiredComponentTypes)
        {
            var implementOnlyIInitializable = system.GetType().GetTypeInfo().ImplementedInterfaces.Count() == 1
                && system.GetType().GetTypeInfo().ImplementedInterfaces.FirstOrDefault() == typeof(IInitializable);

            if (!requiredComponentTypes.Any() && system.GetType().Name != nameof(DebugSystem) && !implementOnlyIInitializable)
                throw new Exception($"You should add required component for the system {system.GetType().Name}");
        }
    }
}
