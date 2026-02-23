using Curupira2D.TexturePacker;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Curupira2D.ECS.Components.Drawables
{
    public sealed class SpriteAnimationTextureAtlasComponent : DrawableComponent
    {
        private Color[] _textureData;

        public SpriteAnimationTextureAtlasComponent(
            Texture2D texture,
            List<TextureAtlas> textureAtlas,
            TimeSpan frameTime,
            bool isLooping = false,
            bool isPlaying = false,
            SpriteEffects spriteEffect = SpriteEffects.None,
            Color color = default,
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawInUICamera = false) : base(texture, spriteEffect, color, null, layerDepth, scale, drawInUICamera)
        {
            TextureAtlases = textureAtlas ?? throw new ArgumentNullException(nameof(textureAtlas));
            FrameTime = frameTime;
            IsLooping = isLooping;
            IsPlaying = isPlaying;
        }

        public SpriteAnimationTextureAtlasComponent(
            Texture2D texture,
            List<TextureAtlas> textureAtlas,
            float frameTimeMilliseconds,
            bool isLooping = false,
            bool isPlaying = false,
            SpriteEffects spriteEffect = SpriteEffects.None,
            Color color = default,
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawInUICamera = false) : this(
                texture,
                textureAtlas,
                TimeSpan.FromMilliseconds(frameTimeMilliseconds),
                isLooping,
                isPlaying,
                spriteEffect,
                color,
                layerDepth,
                scale,
                drawInUICamera)
        { }

        public TimeSpan FrameTime { get; set; }
        public List<TextureAtlas> TextureAtlases { get; set; }
        public bool IsLooping { get; set; }
        public bool IsPlaying { get; set; }
        public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;
        public int CurrentTextureAtlasIndex { get; set; }
        public TextureAtlas CurrentTextureAtlas { get; internal set; }

        public override Color[] TextureData
        {
            get
            {
                if (Texture is null || !SourceRectangle.HasValue)
                    return [];

                _textureData = new Color[SourceRectangle.Value.Width * SourceRectangle.Value.Height];
                Texture.GetData(0, SourceRectangle, _textureData, 0, SourceRectangle.Value.Width * SourceRectangle.Value.Height);

                return _textureData;
            }
        }
    }
}
