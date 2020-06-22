using MonoGame.Helper.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Helper.Asserts
{
    public static class AssertSystem
    {
        public static void AssertRequiredComponents(this ECS.System system, List<Type> requiredComponentTypes)
        {
            if (!requiredComponentTypes.Any() && system.GetType().Name != nameof(DebugSystem))
                throw new Exception($"You should add required component for the system {system.GetType().Name}");
        }
    }
}
