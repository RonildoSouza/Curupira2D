using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Helper.ECS.Components.Drawables
{
    public class SpriteAnimationComponent : SpriteComponent
    {
        public SpriteAnimationComponent(
            Texture2D texture,
            int frameRowsCount,
            int frameColumnsCount,
            TimeSpan frameTime,
            AnimateType animateType = AnimateType.All,
            Rectangle sourceRectangle = default,
            bool isLooping = false,
            bool isPlaying = false,
            SpriteEffects spriteEffect = SpriteEffects.None,
            Color color = default,
            float layerDepth = 0,
            Vector2 scale = default) : base(texture, spriteEffect, color, sourceRectangle, layerDepth, scale)
        {
            FrameRowsCount = frameRowsCount;
            FrameColumnsCount = frameColumnsCount;
            FrameTime = frameTime;
            AnimateType = animateType;
            IsLooping = isLooping;
            IsPlaying = isPlaying;

            if (sourceRectangle == default)
                SourceRectangle = new Rectangle(0, 0,
                    Texture.Bounds.Width / frameColumnsCount,
                    Texture.Bounds.Height / frameRowsCount);
        }

        public SpriteAnimationComponent(
            Texture2D texture,
            int frameRowsCount,
            int frameColumnsCount,
            int frameTimeMilliseconds,
            AnimateType animateType = AnimateType.All,
            Rectangle sourceRectangle = default,
            bool isLooping = false,
            bool isPlaying = false,
            SpriteEffects spriteEffect = SpriteEffects.None,
            Color color = default,
            float layerDepth = 0,
            Vector2 scale = default) : this(
                texture,
                frameRowsCount,
                frameColumnsCount,
                TimeSpan.FromMilliseconds(frameTimeMilliseconds),
                animateType,
                sourceRectangle,
                isLooping,
                isPlaying,
                spriteEffect,
                color,
                layerDepth,
                scale)
        { }

        public int FrameRowsCount { get; set; }
        public int FrameColumnsCount { get; set; }
        public TimeSpan FrameTime { get; set; }
        public bool IsLooping { get; set; }
        public bool IsPlaying { get; set; }
        public AnimateType AnimateType { get; set; }
        public int FrameWidth => TextureSize.X / FrameColumnsCount;
        public int FrameHeight => TextureSize.Y / FrameRowsCount;
        public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;
        public int CurrentFrameColumn { get; set; }
        public int CurrentFrameRow { get; set; }
    }

    public enum AnimateType
    {
        All,
        PerRow,
        PerColumn,
    }
}
