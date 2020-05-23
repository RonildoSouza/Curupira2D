using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Helper.ECS.Components.Drawables
{
    public abstract class DrawableComponent : IComponent
    {
        protected DrawableComponent(
            SpriteEffects spriteEffect = SpriteEffects.None,
            Color color = default,
            Rectangle? sourceRectangle = null,
            float layerDepth = 0f,
            Vector2 scale = default)
        {
            SpriteEffect = spriteEffect;
            Color = color == default ? Color.White : color;
            SourceRectangle = sourceRectangle;
            LayerDepth = layerDepth;
            Scale = scale == default ? Vector2.One : scale;
        }

        public SpriteEffects SpriteEffect { get; set; }
        public Color Color { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public float LayerDepth { get; set; }
        public Vector2 Scale { get; set; }

        //public Rectangle BoundingBox
        //{
        //    get
        //    {
        //        if (Texture == null || Position == null)
        //            throw new NullReferenceException($"Property {nameof(Texture)} or {nameof(Position)} is null!");

        //        return new Rectangle(Position.ToPoint(), Texture.Bounds.Size * Scale.ToPoint());
        //    }
        //}
    }
}
