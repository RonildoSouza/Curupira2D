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
        {
            if (entity?.Components.Any(_ => _.Key == typeof(SpriteComponent) || _.Key == typeof(SpriteAnimationComponent)) ?? false)
                return entity.GetComponent<SpriteComponent>() ?? entity.GetComponent<SpriteAnimationComponent>();

            if (entity?.Components.Any(_ => _.Key == typeof(TextComponent)) ?? false)
                return entity.GetComponent<TextComponent>();

            return null;
        }

        public static Rectangle GetHitBox(this Entity entity)
        {
            if (entity == null)
                return Rectangle.Empty;

            if (entity.Components.Any(_ => _.Key == typeof(SpriteComponent) || _.Key == typeof(SpriteAnimationComponent)))
            {
                var spriteComponent = entity.GetComponent<SpriteComponent>() ?? entity.GetComponent<SpriteAnimationComponent>();
                var size = spriteComponent.SourceRectangle?.Size ?? spriteComponent.TextureSize.ToPoint();
                var x = entity.Position.X - spriteComponent.Origin.X;
                var y = entity.Position.Y - spriteComponent.Origin.Y;

                var position = new Vector2(x, y);

                return new Rectangle(position.ToPoint(), size * spriteComponent.Scale.ToPoint());
            }

            if (entity.Components.Any(_ => _.Key == typeof(TextComponent)))
            {
                var textComponent = entity.GetComponent<TextComponent>();
                var size = textComponent.SourceRectangle?.Size ?? textComponent.TextSize.ToPoint();
                var x = entity.Position.X - textComponent.Origin.X;
                var y = entity.Position.Y - textComponent.Origin.Y;

                var position = new Vector2(x, y);

                return new Rectangle(position.ToPoint(), size * textComponent.Scale.ToPoint());
            }

            return new Rectangle(entity.Position.ToPoint(), Point.Zero);
        }
    }
}
