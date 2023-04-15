using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Curupira2D.ECS.Components.Drawables
{
    public class SpriteAnimationComponent : SpriteComponent
    {
        public SpriteAnimationComponent(
            Texture2D texture,
            int frameRowsCount,
            int frameColumnsCount,
            TimeSpan frameTime,
            AnimateType animateType = AnimateType.All,
            Rectangle? sourceRectangle = null,
            bool isLooping = false,
            bool isPlaying = false,
            SpriteEffects spriteEffect = SpriteEffects.FlipVertically,
            Color color = default,
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawInUICamera = false) : base(texture, spriteEffect, color, sourceRectangle, layerDepth, scale, drawInUICamera)
        {
            FrameRowsCount = frameRowsCount;
            FrameColumnsCount = frameColumnsCount;
            FrameTime = frameTime;
            AnimateType = animateType;
            IsLooping = isLooping;
            IsPlaying = isPlaying;

            if (sourceRectangle == null)
                SourceRectangle = new Rectangle(0, 0, FrameWidth, FrameHeight);

            Origin = Half = new Vector2(FrameWidth * 0.5f, FrameHeight * 0.5f);

            TextureData = new Color[SourceRectangle.Value.Width * SourceRectangle.Value.Height];
            texture.GetData(0, SourceRectangle, TextureData, 0, SourceRectangle.Value.Width * SourceRectangle.Value.Height);
        }

        public SpriteAnimationComponent(
            Texture2D texture,
            int frameRowsCount,
            int frameColumnsCount,
            int frameTimeMilliseconds,
            AnimateType animateType = AnimateType.All,
            Rectangle? sourceRectangle = null,
            bool isLooping = false,
            bool isPlaying = false,
            SpriteEffects spriteEffect = SpriteEffects.FlipVertically,
            Color color = default,
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawInUICamera = false) : this(
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
                scale,
                drawInUICamera)
        { }

        public int FrameRowsCount { get; set; }
        public int FrameColumnsCount { get; set; }
        public TimeSpan FrameTime { get; set; }
        public bool IsLooping { get; set; }
        public bool IsPlaying { get; set; }
        public AnimateType AnimateType { get; set; }
        public int FrameWidth => (int)TextureSize.X / FrameColumnsCount;
        public int FrameHeight => (int)TextureSize.Y / FrameRowsCount;
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
