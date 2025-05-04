using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Curupira2D.ECS.Components.Drawables
{
    public sealed class SpriteAnimationComponent : DrawableComponent
    {
        private Texture2D _texture;
        private Color[] _textureData;

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
            bool drawInUICamera = false,
            Vector2 textureSizeOffset = default) : base(texture, spriteEffect, color, sourceRectangle, layerDepth, scale, drawInUICamera)
        {
            FrameRowsCount = frameRowsCount;
            FrameColumnsCount = frameColumnsCount;
            FrameTime = frameTime;
            AnimateType = animateType;
            IsLooping = isLooping;
            IsPlaying = isPlaying;

            if (sourceRectangle == null)
                SourceRectangle = new Rectangle(0, 0, FrameWidth, FrameHeight);

            if (textureSizeOffset == default)
                textureSizeOffset = Vector2.Zero;

            TextureSizeOffset = textureSizeOffset;
            Origin = new Vector2(FrameWidth * 0.5f, FrameHeight * 0.5f);
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
            bool drawInUICamera = false,
            Vector2 textureSizeOffset = default) : this(
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
                drawInUICamera,
                textureSizeOffset)
        { }

        public int FrameRowsCount { get; set; }
        public int FrameColumnsCount { get; set; }
        public TimeSpan FrameTime { get; set; }
        public bool IsLooping { get; set; }
        public bool IsPlaying { get; set; }
        public AnimateType AnimateType { get; set; }
        public int FrameWidth => (int)(TextureSize.X - TextureSizeOffset.X) / FrameColumnsCount;
        public int FrameHeight => (int)(TextureSize.Y - TextureSizeOffset.Y) / FrameRowsCount;
        public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;
        public int CurrentFrameColumn { get; set; }
        public int CurrentFrameRow { get; set; }
        public Vector2 TextureSizeOffset { get; set; }

        public override Texture2D Texture
        {
            get => _texture;
            set
            {
                _texture = value;

                if (FrameColumnsCount > 0 && FrameRowsCount > 0)
                    Origin = new Vector2(FrameWidth * 0.5f, FrameHeight * 0.5f);
            }
        }
        public override Color[] TextureData
        {
            get
            {
                if (_texture is null)
                    return [];

                _textureData = new Color[SourceRectangle.Value.Width * SourceRectangle.Value.Height];
                _texture.GetData(0, SourceRectangle, _textureData, 0, SourceRectangle.Value.Width * SourceRectangle.Value.Height);

                return _textureData;
            }
        }
    }

    public enum AnimateType
    {
        All,
        PerRow,
        PerColumn,
    }
}
