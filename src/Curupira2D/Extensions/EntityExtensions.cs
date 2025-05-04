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

        public static DrawableComponent GetDrawableComponent(this Entity entity)
            => entity.GetComponent(_ => _.Value is DrawableComponent) as DrawableComponent;

        public static Rectangle GetHitBox(this Entity entity)
        {
            if (entity == null)
                return Rectangle.Empty;

            if (entity.Components.Any(_ => _.Key == typeof(SpriteComponent) || _.Key == typeof(SpriteAnimationComponent)))
            {
                var spriteComponent = entity.GetComponent(_ => _.Value is DrawableComponent) as DrawableComponent;
                return RectangleHitBoxBuilder(spriteComponent, entity, spriteComponent.TextureSize.ToPoint());
            }

            if (entity.Components.Any(_ => _.Key == typeof(TextComponent)))
            {
                var textComponent = entity.GetComponent<TextComponent>();
                return RectangleHitBoxBuilder(textComponent, entity, textComponent.TextSize.ToPoint());
            }

            return new Rectangle(entity.Position.ToPoint(), Point.Zero);

            static Rectangle RectangleHitBoxBuilder(DrawableComponent component, Entity entity, Point sizeIfNullSourceRectangle)
            {
                var size = component.SourceRectangle?.Size ?? sizeIfNullSourceRectangle;
                var x = entity.Position.X - (component.Origin.X * component.Scale.X);
                var y = entity.Position.Y - (component.Origin.Y * component.Scale.Y);

                var position = new Vector2(x, y);
                var sizeToPoint = (size.ToVector2() * component.Scale).ToPoint();

                return new Rectangle(position.ToPoint(), sizeToPoint);
            }
        }
    }
}
