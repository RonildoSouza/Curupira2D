﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.ECS.Components.Drawables
{
    public class SpriteComponent : DrawableComponent
    {
        public SpriteComponent(
            Texture2D texture,
            SpriteEffects spriteEffect = SpriteEffects.FlipVertically,
            Color color = default,
            Rectangle? sourceRectangle = null,
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawInUICamera = false) : base(spriteEffect, color, sourceRectangle, layerDepth, scale, drawInUICamera)
        {
            Texture = texture;
            Origin = SourceRectangle.HasValue ? SourceRectangle.Value.Size.ToVector2() * 0.5f : TextureSize * 0.5f;

            TextureData = new Color[(int)(TextureSize.X * TextureSize.Y)];
            texture.GetData(TextureData);
        }

        public Texture2D Texture { get; set; }
        public Vector2 TextureSize => Texture.Bounds.Size.ToVector2();
    }
}
