using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

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

        /// <summary>
        /// Return position with monogame coords
        /// </summary>
        /// <param name="entity"><see cref="Entity"/></param>
        /// <param name="scene"><see cref="Scene"/></param>
        public static Vector2 GetPositionInScene(this Entity entity, Scene scene)
        {
            if (entity == null)
                throw new ArgumentNullException($"Argument {nameof(entity)} can't be null!");

            if (!entity.HasComponent(_ => _.Value is DrawableComponent))
                return entity.Position;

            var drawableComponent = entity.GetComponent(_ => _.Value is DrawableComponent) as DrawableComponent;

            var position = entity.Position;
            position.X -= drawableComponent.Origin.X;
            position.Y = scene.InvertPositionY(position.Y) - drawableComponent.Origin.Y;

            return position;
        }

        public static bool IsCollidedWithAny(this Entity entity, Scene scene)
            => IsCollidedWith(entity, scene, _ => _.GetHitBox().Intersects(entity.GetHitBox()));

        public static bool IsCollidedWithAny(this Entity entity, Scene scene, string entityGroup)
            => IsCollidedWith(entity, scene, _ => _.Group == entityGroup && _.GetHitBox().Intersects(entity.GetHitBox()));

        public static bool IsCollidedWith(this Entity entity, Scene scene, Entity otherEntity)
            => IsCollidedWith(entity, scene, _ => _.Equals(otherEntity) && _.GetHitBox().Intersects(entity.GetHitBox()));

        public static bool IsCollidedWith(this Entity entity, Scene scene, string otherEntityUniqueId)
            => IsCollidedWith(entity, scene, _ => _.UniqueId == otherEntityUniqueId && _.GetHitBox().Intersects(entity.GetHitBox()));

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

        private static bool IsCollidedWith(Entity entity, Scene scene, Func<Entity, bool> predicate)
        {
            if (!entity.Active || !entity.IsCollidable)
                return false;

            scene.Quadtree.Delete(entity);
            scene.Quadtree.Insert(entity);

            var returnObjects = scene.Quadtree.Retrieve(entity);
            return returnObjects.Any(predicate);
        }
    }
}
