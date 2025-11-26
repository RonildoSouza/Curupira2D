using Curupira2D.TexturePacker;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Curupira2D.ECS.Components.Drawables
{
    public sealed class SpriteAnimationTextureAtlasComponent : DrawableComponent
    {
        //private Texture2D _texture;
        //private Color[] _textureData;

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

            //Origin = new Vector2(FrameWidth * 0.5f, FrameHeight * 0.5f);
        }

        //public SpriteAnimationTextureAtlasComponent(
        //    Texture2D texture,
        //    int frameRowsCount,
        //    int frameColumnsCount,
        //    int frameTimeMilliseconds,
        //    AnimateType animateType = AnimateType.All,
        //    Rectangle? sourceRectangle = null,
        //    bool isLooping = false,
        //    bool isPlaying = false,
        //    SpriteEffects spriteEffect = SpriteEffects.None,
        //    Color color = default,
        //    float layerDepth = 0f,
        //    Vector2 scale = default,
        //    bool drawInUICamera = false,
        //    Vector2 textureSizeOffset = default) : this(
        //        texture,
        //        frameRowsCount,
        //        frameColumnsCount,
        //        TimeSpan.FromMilliseconds(frameTimeMilliseconds),
        //        animateType,
        //        sourceRectangle,
        //        isLooping,
        //        isPlaying,
        //        spriteEffect,
        //        color,
        //        layerDepth,
        //        scale,
        //        drawInUICamera,
        //        textureSizeOffset)
        //{ }

        public TimeSpan FrameTime { get; set; }
        public List<TextureAtlas> TextureAtlases { get; set; }
        public bool IsLooping { get; set; }
        public bool IsPlaying { get; set; }
        //public int FrameWidth => (int)(TextureSize.X - TextureSizeOffset.X) / FrameColumnsCount;
        //public int FrameHeight => (int)(TextureSize.Y - TextureSizeOffset.Y) / FrameRowsCount;
        public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;

        public int CurrentTextureAtlasIndex { get; set; }
        public int TotalTextureAtlases => TextureAtlases.Count;
        //public Vector2 TextureSizeOffset { get; set; }

        //public override Texture2D Texture
        //{
        //    get => _texture;
        //    set
        //    {
        //        _texture = value;

        //        if (FrameColumnsCount > 0 && FrameRowsCount > 0)
        //            Origin = new Vector2(FrameWidth * 0.5f, FrameHeight * 0.5f);
        //    }
        //}

        public override Color[] TextureData => throw new NotImplementedException();
        //public override Color[] TextureData
        //{
        //    get
        //    {
        //        if (_texture is null)
        //            return [];

        //        _textureData = new Color[SourceRectangle.Value.Width * SourceRectangle.Value.Height];
        //        _texture.GetData(0, SourceRectangle, _textureData, 0, SourceRectangle.Value.Width * SourceRectangle.Value.Height);

        //        return _textureData;
        //    }
        //}
    }
}
