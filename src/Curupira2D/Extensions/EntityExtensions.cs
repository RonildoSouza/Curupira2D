using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curupira2D.Extensions
{
    public static class EntityExtensions
    {
        public static void WithScreenBoundaryNotExit(this Entity entity, GraphicsDevice graphicsDevice)
        {
            entity.Position.X = MathHelper.Clamp(entity.Position.X, entity.Origin.X,
                graphicsDevice.Viewport.Width - entity.BoundingBox.Width + entity.Origin.X);

            entity.Position.Y = MathHelper.Clamp(entity.Position.Y, entity.Origin.Y,
                graphicsDevice.Viewport.Height - entity.BoundingBox.Height + entity.Origin.Y);
        }

        public static bool WithScreenBoundary(this Entity entity, GraphicsDevice graphicsDevice)
        {
            var collided = false;

            if (entity.BoundingBox.X <= 0 ||
                entity.BoundingBox.Y >= (graphicsDevice.Viewport.Height - entity.BoundingBox.Height) ||
                entity.BoundingBox.X >= (graphicsDevice.Viewport.Width - entity.BoundingBox.Width) ||
                entity.BoundingBox.Y <= 0)
                collided = true;

            return collided;
        }
    }
}
