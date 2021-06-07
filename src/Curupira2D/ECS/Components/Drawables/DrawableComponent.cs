using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.ECS.Components.Drawables
{
    public abstract class DrawableComponent : IComponent
    {
        private bool _drawUsingCamera;

        protected DrawableComponent(
            SpriteEffects spriteEffect = SpriteEffects.FlipVertically,
            Color color = default,
            Rectangle? sourceRectangle = null,
            float layerDepth = 0f,
            Vector2 scale = default,
            bool drawWithoutUsingCamera = true)
        {
            SpriteEffect = spriteEffect;
            Color = color == default ? Color.White : color;
            SourceRectangle = sourceRectangle;
            LayerDepth = layerDepth;
            Scale = scale == default ? Vector2.One : scale;
            DrawWithoutUsingCamera = drawWithoutUsingCamera;
        }

        public abstract Vector2 Origin { get; }
        public SpriteEffects SpriteEffect { get; set; }
        public Color Color { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public float LayerDepth { get; set; }
        public Vector2 Scale { get; set; }
        public bool DrawWithoutUsingCamera
        {
            get => _drawUsingCamera;
            set
            {
                _drawUsingCamera = value;

                if (_drawUsingCamera)
                    SpriteEffect = SpriteEffects.None;
            }
        }
    }
}
