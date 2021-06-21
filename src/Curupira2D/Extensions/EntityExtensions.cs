using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Microsoft.Xna.Framework;
using System;

namespace Curupira2D.Extensions
{
    public static class EntityExtensions
    {
        public static Entity AddComponent(this Entity entity, DrawableComponent drawableComponent, BodyComponent bodyComponent)
        {
            if (drawableComponent == null || bodyComponent == null)
                throw new ArgumentNullException($"Argument {nameof(drawableComponent)} or {nameof(bodyComponent)} can't be null!");

            // This is necessary because Aether.Physics2D works better with negative Y gravity.
            // See samples: https://github.com/tainicom/Aether.Physics2D/tree/master/Samples
            drawableComponent.DrawInUICamera = false;
            entity.AddComponent(drawableComponent).AddComponent(bodyComponent);

            return entity;
        }

        public static Vector2 GetPositionInScene(this Entity entity, Scene scene)
        {
            if (entity == null)
                throw new ArgumentNullException($"Argument {nameof(entity)} can't be null!");

            if (!entity.HasComponent(_ => _.Value is DrawableComponent))
                return entity.Transform.Position;

            var drawableComponent = entity.GetComponent(_ => _.Value is DrawableComponent) as DrawableComponent;

            var position = entity.Transform.Position;
            position.X -= drawableComponent.Origin.X;
            position.Y = scene.InvertPositionY(position.Y) - drawableComponent.Origin.Y;

            return position;
        }

        //public static void WithScreenBoundaryNotExit(this Entity entity, GraphicsDevice graphicsDevice)
        //{
        //    entity.Position.X = MathHelper.Clamp(entity.Position.X, entity.Origin.X,
        //        graphicsDevice.Viewport.Width - entity.BoundingBox.Width + entity.Origin.X);

        //    entity.Position.Y = MathHelper.Clamp(entity.Position.Y, entity.Origin.Y,
        //        graphicsDevice.Viewport.Height - entity.BoundingBox.Height + entity.Origin.Y);
        //}

        //public static bool WithScreenBoundary(this Entity entity, GraphicsDevice graphicsDevice)
        //{
        //    var collided = false;

        //    if (entity.BoundingBox.X <= 0 ||
        //        entity.BoundingBox.Y >= (graphicsDevice.Viewport.Height - entity.BoundingBox.Height) ||
        //        entity.BoundingBox.X >= (graphicsDevice.Viewport.Width - entity.BoundingBox.Width) ||
        //        entity.BoundingBox.Y <= 0)
        //        collided = true;

        //    return collided;
        //}
    }
}
