using Curupira2D.ECS;
using System.Collections.Generic;
using System.Linq;
using nkast.Aether.Physics2D.Dynamics;

namespace Curupira2D.Extensions
{
    public static class BodyExtensions
    {
        public static Body GetEntityBody(this List<Body> bodies, Entity entity)
        {
            if (!bodies.Any() || entity == null)
                return null;

            return bodies.FirstOrDefault(_ => _.Tag is string && _.Tag.ToString() == entity.UniqueId);
        }
    }
}
