using Curupira2D.ECS.Components.Drawables;
using Curupira2D.TexturePacker;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.Extensions.TexturePacker
{
    public static class TexturePackerDataExtensions
    {
        public static SpriteComponent ToSpriteComponent(
            this TextureAtlas textureAtlas,
            Texture2D texture,
            SpriteEffects spriteEffect = SpriteEffects.None,
            Color color = default,
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawInUICamera = false)
        {
            var sourceRectangle = textureAtlas.Frame.ToRectangle();
            return new SpriteComponent(texture, spriteEffect, color, sourceRectangle, layerDepth, scale, drawInUICamera);
        }

        public static SpriteComponent ToSpriteComponentByName(
            this TexturePackerData texturePackerData,
            string filename,
            Texture2D texture,
            SpriteEffects spriteEffect = SpriteEffects.None,
            Color color = default,
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawInUICamera = false)
        {
            if (string.IsNullOrEmpty(filename))
                ArgumentNullException.ThrowIfNull($"Argument {nameof(filename)} can't be null or empty!");

            var textureAtlas = texturePackerData.GetByName(filename);
            return textureAtlas.ToSpriteComponent(texture, spriteEffect, color, layerDepth, scale, drawInUICamera);
        }

        public static Dictionary<string, SpriteComponent> ToSpriteComponentWithRegex(
            this TexturePackerData texturePackerData,
            string regexPattern,
            Texture2D texture,
            SpriteEffects spriteEffect = SpriteEffects.None,
            Color color = default,
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawInUICamera = false)
        {
            if (string.IsNullOrEmpty(regexPattern))
                ArgumentNullException.ThrowIfNull($"Argument {nameof(regexPattern)} can't be null or empty!");

            var textureAtlas = texturePackerData.GetWithRegex(regexPattern);
            return textureAtlas.ToDictionary(
                _ => _.Filename,
                _ => _.ToSpriteComponent(texture, spriteEffect, color, layerDepth, scale, drawInUICamera));
        }
    }
}
