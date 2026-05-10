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
                ArgumentNullException.ThrowIfNull($"Argument {nameof(drawableComponent)} or {nameof(bodyComponent)} can't be null!");

            entity.AddComponent(drawableComponent).AddComponent(bodyComponent);

            return entity;
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

            if (entity.Components.Any(_ => _.Key == typeof(BodyComponent)))
            {
                var position = Vector2.Zero;

                var bodyComponent = entity.GetComponent<BodyComponent>();

                if (bodyComponent.EntityShape == EntityShape.Circle)
                {
                    var radius = bodyComponent.Radius;
                    var diameter = radius * 2f;
                    position = new Vector2(entity.Position.X - radius, entity.Position.Y - radius);

                    return new Rectangle(position.ToPoint(), new Point((int)diameter, (int)diameter));
                }

                if (bodyComponent.EntityShape == EntityShape.Ellipse)
                {
                    var radiusX = bodyComponent.Size.X * 0.5f;
                    var radiusY = bodyComponent.Size.Y * 0.5f;
                    var size = new Point((int)(radiusX * 2f), (int)(radiusY * 2f));
                    position = new Vector2(entity.Position.X - radiusX, entity.Position.Y - radiusY);

                    return new Rectangle(position.ToPoint(), size);
                }

                position = new Vector2(entity.Position.X - bodyComponent.Size.X * 0.5f, entity.Position.Y - bodyComponent.Size.Y * 0.5f);
                return new Rectangle(position.ToPoint(), bodyComponent.Size.ToPoint());
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
