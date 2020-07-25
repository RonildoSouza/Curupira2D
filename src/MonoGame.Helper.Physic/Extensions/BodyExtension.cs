using MonoGame.Helper.ECS;
using System.Collections.Generic;
using System.Linq;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGame.Helper.Physic.Extensions
{
    public static class BodyExtension
    {
        public static Body GetEntityBody(this List<Body> bodies, Entity entity)
        {
            if (!bodies.Any() || entity == null)
                return null;

            return bodies.FirstOrDefault(_ => _.Tag is string && _.Tag.ToString() == entity.UniqueId);
        }
    }
}
