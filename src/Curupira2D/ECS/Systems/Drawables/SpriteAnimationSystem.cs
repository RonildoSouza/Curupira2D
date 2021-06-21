using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Curupira2D.ECS.Systems.Drawables
{
    [RequiredComponent(typeof(SpriteAnimationSystem), typeof(SpriteAnimationComponent))]
    public sealed class SpriteAnimationSystem : System, IRenderable
    {
        public void Draw(ref IReadOnlyList<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var spriteAnimationComponent = entity.GetComponent<SpriteAnimationComponent>();

                Animate(ref spriteAnimationComponent);
                Scene.SpriteBatch.Draw(entity, spriteAnimationComponent);
            }
        }

        void Animate(ref SpriteAnimationComponent spriteAnimationComponent)
        {
            if (spriteAnimationComponent.SourceRectangle == Rectangle.Empty || spriteAnimationComponent.SourceRectangle.Value.Width == 0 || spriteAnimationComponent.SourceRectangle.Value.Height == 0)
                throw new ArgumentException($"The argument {nameof(spriteAnimationComponent.SourceRectangle)} cannot be Empty or Width or Height be equals Zero!");

            if (!spriteAnimationComponent.IsPlaying)
                return;

            spriteAnimationComponent.ElapsedTime += (Scene.GameTime?.ElapsedGameTime).GetValueOrDefault();

            if (spriteAnimationComponent.ElapsedTime >= spriteAnimationComponent.FrameTime)
            {
                switch (spriteAnimationComponent.AnimateType)
                {
                    case AnimateType.All:
                        AnimateAll(ref spriteAnimationComponent);
                        break;
                    case AnimateType.PerRow:
                        AnimatePerRow(ref spriteAnimationComponent);
                        break;
                    case AnimateType.PerColumn:
                        AnimatePerColumn(ref spriteAnimationComponent);
                        break;
                }

                spriteAnimationComponent.ElapsedTime = TimeSpan.Zero;
            }
        }

        void AnimateAll(ref SpriteAnimationComponent spriteAnimationComponent)
        {
            spriteAnimationComponent.CurrentFrameColumn++;

            if (spriteAnimationComponent.CurrentFrameColumn == spriteAnimationComponent.FrameColumnsCount)
            {
                spriteAnimationComponent.CurrentFrameColumn = 0;
                spriteAnimationComponent.CurrentFrameRow++;

                if (spriteAnimationComponent.CurrentFrameRow == spriteAnimationComponent.FrameRowsCount)
                {
                    spriteAnimationComponent.CurrentFrameRow = 0;
                    spriteAnimationComponent.IsPlaying = spriteAnimationComponent.IsLooping;
                }
            }

            spriteAnimationComponent.SourceRectangle = new Rectangle(
                spriteAnimationComponent.CurrentFrameColumn * spriteAnimationComponent.FrameWidth,
                spriteAnimationComponent.CurrentFrameRow * spriteAnimationComponent.FrameHeight,
                spriteAnimationComponent.FrameWidth,
                spriteAnimationComponent.FrameHeight);
        }

        void AnimatePerRow(ref SpriteAnimationComponent spriteAnimationComponent)
        {
            spriteAnimationComponent.CurrentFrameColumn++;

            if (spriteAnimationComponent.CurrentFrameColumn == spriteAnimationComponent.FrameColumnsCount)
            {
                spriteAnimationComponent.CurrentFrameColumn = 0;
                spriteAnimationComponent.IsPlaying = spriteAnimationComponent.IsLooping;
            }

            spriteAnimationComponent.SourceRectangle = new Rectangle(
                    spriteAnimationComponent.CurrentFrameColumn * spriteAnimationComponent.FrameWidth,
                    spriteAnimationComponent.SourceRectangle.Value.Y,
                    spriteAnimationComponent.FrameWidth,
                    spriteAnimationComponent.SourceRectangle.Value.Height);
        }

        void AnimatePerColumn(ref SpriteAnimationComponent spriteAnimationComponent)
        {
            spriteAnimationComponent.CurrentFrameRow++;

            if (spriteAnimationComponent.CurrentFrameRow == spriteAnimationComponent.FrameRowsCount)
            {
                spriteAnimationComponent.CurrentFrameRow = 0;
                spriteAnimationComponent.IsPlaying = spriteAnimationComponent.IsLooping;
            }

            spriteAnimationComponent.SourceRectangle = new Rectangle(
                    spriteAnimationComponent.SourceRectangle.Value.X,
                    spriteAnimationComponent.CurrentFrameRow * spriteAnimationComponent.FrameHeight,
                    spriteAnimationComponent.SourceRectangle.Value.Width,
                    spriteAnimationComponent.FrameHeight);
        }
    }
}