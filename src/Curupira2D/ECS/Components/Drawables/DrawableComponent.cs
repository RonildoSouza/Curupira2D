using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.ECS.Components.Drawables
{
    public abstract class DrawableComponent : IComponent
    {
        protected DrawableComponent(
            SpriteEffects spriteEffect = SpriteEffects.FlipVertically,
            Color color = default,
            Rectangle? sourceRectangle = null,
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawInUICamera = false)
        {
            SpriteEffect = spriteEffect;
            Color = color == default ? Color.White : color;
            SourceRectangle = sourceRectangle;
            LayerDepth = layerDepth;
            Scale = scale == default ? Vector2.One : scale;
            DrawInUICamera = drawInUICamera;
        }

        public Vector2 Origin { get; set; }
        public SpriteEffects SpriteEffect { get; set; }
        public Color Color { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public virtual float LayerDepth { get; set; }
        public Vector2 Scale { get; set; }
        public bool DrawInUICamera { get; set; }
        public Vector2? Position { get; set; }
        public abstract Color[] TextureData { get; }
    }
}
