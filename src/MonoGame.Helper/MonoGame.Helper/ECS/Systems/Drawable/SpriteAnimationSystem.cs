using Microsoft.Xna.Framework;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.Extensions;
using System;
using System.Linq;

namespace MonoGame.Helper.ECS.Systems.Drawable
{
    [RequiredComponent(typeof(SpriteAnimationComponent))]
    public class SpriteAnimationSystem : System, IRenderable
    {
        TimeSpan _elapsedTime = TimeSpan.Zero;
        int _currentFrameColumn;
        int _currentFrameRow;

        public void Draw()
        {
            var entities = Scene.GetEntities(_ => Matches(_));

            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities.ElementAt(i);
                var spriteAnimationComponent = entity.GetComponent<SpriteAnimationComponent>();

                Animate(ref spriteAnimationComponent);

                Scene.SpriteBatch.Draw(entity.Transform.Position, entity.Transform.RotationInDegrees, spriteAnimationComponent);
            }
        }

        private void Animate(ref SpriteAnimationComponent spriteAnimationComponent)
        {
            if (spriteAnimationComponent.SourceRectangle == Rectangle.Empty || spriteAnimationComponent.SourceRectangle.Value.Width == 0 || spriteAnimationComponent.SourceRectangle.Value.Height == 0)
                throw new ArgumentException($"The argument {nameof(spriteAnimationComponent.SourceRectangle)} cannot be Empty or Width or Height be equals Zero!");

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
        }

        private void AnimateAll(ref SpriteAnimationComponent spriteAnimationComponent)
        {
            if (!spriteAnimationComponent.IsPlaying)
            {
                spriteAnimationComponent.SourceRectangle = new Rectangle(
                    _currentFrameColumn * spriteAnimationComponent.FrameWidth,
                    _currentFrameRow * spriteAnimationComponent.FrameHeight,
                    spriteAnimationComponent.FrameWidth,
                    spriteAnimationComponent.FrameHeight);
                return;
            }

            _elapsedTime += Scene.GameTime.ElapsedGameTime;

            if (_elapsedTime >= spriteAnimationComponent.FrameTime)
            {
                _currentFrameColumn++;

                if (_currentFrameColumn == spriteAnimationComponent.FrameColumnsCount)
                {
                    _currentFrameColumn = 0;
                    _currentFrameRow++;

                    if (_currentFrameRow == spriteAnimationComponent.FrameRowsCount)
                    {
                        _currentFrameRow = 0;
                        spriteAnimationComponent.IsPlaying = spriteAnimationComponent.IsLooping;
                    }
                }

                _elapsedTime = TimeSpan.Zero;
            }

            spriteAnimationComponent.SourceRectangle = new Rectangle(
                _currentFrameColumn * spriteAnimationComponent.FrameWidth,
                _currentFrameRow * spriteAnimationComponent.FrameHeight,
                spriteAnimationComponent.FrameWidth,
                spriteAnimationComponent.FrameHeight);
        }

        private void AnimatePerRow(ref SpriteAnimationComponent spriteAnimationComponent)
        {
            if (!spriteAnimationComponent.IsPlaying)
            {
                spriteAnimationComponent.SourceRectangle = new Rectangle(
                    _currentFrameColumn * spriteAnimationComponent.FrameWidth,
                    spriteAnimationComponent.SourceRectangle.Value.Y,
                    spriteAnimationComponent.FrameWidth,
                    spriteAnimationComponent.SourceRectangle.Value.Height);
                return;
            }

            UpdateFramePerRowOrPerColumn(ref spriteAnimationComponent, ref _currentFrameColumn, spriteAnimationComponent.FrameColumnsCount);

            spriteAnimationComponent.SourceRectangle = new Rectangle(
                    _currentFrameColumn * spriteAnimationComponent.FrameWidth,
                    spriteAnimationComponent.SourceRectangle.Value.Y,
                    spriteAnimationComponent.FrameWidth,
                    spriteAnimationComponent.SourceRectangle.Value.Height);
        }

        private void AnimatePerColumn(ref SpriteAnimationComponent spriteAnimationComponent)
        {
            if (!spriteAnimationComponent.IsPlaying)
            {
                spriteAnimationComponent.SourceRectangle = new Rectangle(
                    spriteAnimationComponent.SourceRectangle.Value.X,
                    _currentFrameRow * spriteAnimationComponent.FrameHeight,
                    spriteAnimationComponent.SourceRectangle.Value.Width,
                    spriteAnimationComponent.FrameHeight);
                return;
            }

            UpdateFramePerRowOrPerColumn(ref spriteAnimationComponent, ref _currentFrameRow, spriteAnimationComponent.FrameRowsCount);

            spriteAnimationComponent.SourceRectangle = new Rectangle(
                    spriteAnimationComponent.SourceRectangle.Value.X,
                    _currentFrameRow * spriteAnimationComponent.FrameHeight,
                    spriteAnimationComponent.SourceRectangle.Value.Width,
                    spriteAnimationComponent.FrameHeight);
        }

        private void UpdateFramePerRowOrPerColumn(ref SpriteAnimationComponent spriteAnimationComponent, ref int currentFrame, int frameCount)
        {
            _elapsedTime += Scene.GameTime.ElapsedGameTime;

            if (_elapsedTime >= spriteAnimationComponent.FrameTime)
            {
                currentFrame++;

                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    spriteAnimationComponent.IsPlaying = spriteAnimationComponent.IsLooping;
                }

                _elapsedTime = TimeSpan.Zero;
            }
        }
    }
}

/*
 private void AnimatePerRow(ref SpriteAnimationComponent spriteAnimationComponent, int sourcePosY, int sourceHeight)
        {
            if (!spriteAnimationComponent.IsPlaying)
            {
                spriteAnimationComponent.SourceRectangle = new Rectangle(
                    _currentFrameColumn * spriteAnimationComponent.FrameWidth,
                    sourcePosY,
                    spriteAnimationComponent.FrameWidth,
                    sourceHeight);
                return;
            }

            UpdateFrame(ref spriteAnimationComponent, ref _currentFrameColumn, spriteAnimationComponent.FrameColumnsCount);

            spriteAnimationComponent.SourceRectangle = new Rectangle(
                    _currentFrameColumn * spriteAnimationComponent.FrameWidth,
                    sourcePosY,
                    spriteAnimationComponent.FrameWidth,
                    sourceHeight);
        }

        private void AnimatePerColumn(ref SpriteAnimationComponent spriteAnimationComponent, int sourcePosX, int sourceWidth)
        {
            if (!spriteAnimationComponent.IsPlaying)
            {
                spriteAnimationComponent.SourceRectangle = new Rectangle(
                    sourcePosX,
                    _currentFrameRow * spriteAnimationComponent.FrameHeight,
                    sourceWidth,
                    spriteAnimationComponent.FrameHeight);
                return;
            }

            UpdateFrame(ref spriteAnimationComponent, ref _currentFrameRow, spriteAnimationComponent.FrameRowsCount);

            spriteAnimationComponent.SourceRectangle = new Rectangle(
                    sourcePosX,
                    _currentFrameRow * spriteAnimationComponent.FrameHeight,
                    sourceWidth,
                    spriteAnimationComponent.FrameHeight);
        }
 
 */
