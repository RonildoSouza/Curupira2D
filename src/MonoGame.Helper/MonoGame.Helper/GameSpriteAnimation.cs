﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace MonoGame.Helper
{
    public class GameSpriteAnimation : GameSprite
    {
        TimeSpan _elapsedTime;
        TimeSpan _frameTime;
        int _frameRowsCount;
        int _frameColumnsCount;
        int _currentFrameColumn;
        int _currentFrameRow;

        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public bool IsLooping { get; set; }
        public bool IsPlaying { get; private set; }

        /// <param name="assetName">Sprite sheet name.</param>
        /// <param name="frameRowsCount">Number of rows in the sprite sheet.</param>
        /// <param name="frameColumnsCount">Number of columns in the sprite sheet.</param>
        /// <param name="frameTime">Frame rendering time.</param>
        /// <param name="sourceRectangle">Size of frame.</param>
        /// <param name="isLooping">Anima sprite sheet in loop.</param>
        public GameSpriteAnimation(string assetName, int frameRowsCount, int frameColumnsCount, TimeSpan frameTime, Rectangle sourceRectangle, bool isLooping = true) : base(assetName)
        {
            if (sourceRectangle == Rectangle.Empty || sourceRectangle.Width.Equals(0) || sourceRectangle.Height.Equals(0))
                throw new ArgumentException("sourceRectangle cannot be Empty or sourceRectangle.Width or sourceRectangle.Height be equals Zero!");

            _frameRowsCount = frameRowsCount;
            _frameColumnsCount = frameColumnsCount;
            _frameTime = frameTime;
            IsLooping = isLooping;
            SourceRectangle = sourceRectangle;

            _elapsedTime = TimeSpan.Zero;
            _currentFrameColumn = 0;
            _currentFrameRow = 0;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            try
            {
                FrameWidth = BoundingBox.Width / _frameColumnsCount;
                FrameHeight = BoundingBox.Height / _frameRowsCount;
            }
            catch (NullReferenceException e)
            {
                System.Diagnostics.Debug.WriteLine($"{e.Message}");
            }
        }

        /// <summary>
        /// Anima all rows and columns frames.
        /// </summary>
        /// <param name="gameTime"></param>
        public void AnimateAll(GameTime gameTime)
        {
            if (!IsPlaying)
            {
                SetSourceRectangle(_currentFrameColumn * FrameWidth, _currentFrameRow * FrameHeight, FrameWidth, FrameHeight);
                return;
            }

            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime >= _frameTime)
            {
                _currentFrameColumn++;

                if (_currentFrameColumn == _frameColumnsCount)
                {
                    _currentFrameColumn = 0;
                    _currentFrameRow++;

                    if (_currentFrameRow == _frameRowsCount)
                    {
                        _currentFrameRow = 0;
                        IsPlaying = IsLooping;
                    }
                }

                _elapsedTime = TimeSpan.Zero;
            }

            SetSourceRectangle(_currentFrameColumn * FrameWidth, _currentFrameRow * FrameHeight, FrameWidth, FrameHeight);
        }

        /// <summary>
        /// Anima sprite per row frame.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="sourcePosY"></param>
        /// <param name="sourceHeight"></param>
        public void AnimatePerRow(GameTime gameTime, int sourcePosY, int sourceHeight)
        {
            if (!IsPlaying)
            {
                SetSourceRectangle(_currentFrameColumn * FrameWidth, sourcePosY, FrameWidth, sourceHeight);
                return;
            }

            UpdateFrame(ref gameTime, ref _currentFrameColumn, ref _frameColumnsCount);
            SetSourceRectangle(_currentFrameColumn * FrameWidth, sourcePosY, FrameWidth, sourceHeight);
        }

        /// <summary>
        /// Anima sprite per column frame.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="sourcePosX"></param>
        /// <param name="sourceWidth"></param>
        public void AnimatePerColumn(GameTime gameTime, int sourcePosX, int sourceWidth)
        {
            if (!IsPlaying)
            {
                SetSourceRectangle(sourcePosX, _currentFrameRow * FrameHeight, sourceWidth, FrameHeight);
                return;
            }

            UpdateFrame(ref gameTime, ref _currentFrameRow, ref _frameRowsCount);
            SetSourceRectangle(sourcePosX, _currentFrameRow * FrameHeight, sourceWidth, FrameHeight);
        }

        public void Reset()
        {
            _currentFrameRow = 0;
            _currentFrameColumn = 0;
            _elapsedTime = TimeSpan.Zero;
            IsPlaying = true;
        }

        public void Pause() => IsPlaying = false;

        public void Play() => IsPlaying = true;

        #region Private Methods
        private void SetSourceRectangle(int x, int y, int width, int height)
            => SourceRectangle = new Rectangle(x, y, width, height);

        private void UpdateFrame(ref GameTime gameTime, ref int currentFrame, ref int frameCount)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime >= _frameTime)
            {
                currentFrame++;

                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    IsPlaying = IsLooping;
                }

                _elapsedTime = TimeSpan.Zero;
            }
        }
        #endregion
    }
}
