using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.Core;
using System;

namespace MonoGame.Helper
{
    public class GameSprite : GameObject2D
    {
        protected string _assetName;

        public Texture2D Texture { get; set; }
        public SpriteEffects SpriteEffect { get; set; }
        public Color Color { get; set; }
        /// <summary>
        /// An optional region on the texture which will be rendered.
        /// </summary>
        public Rectangle? SourceRectangle { get; set; }
        public float LayerDepth { get; set; }
        /// <summary>
        /// Bounding box for represent object limits considering <see cref="GameObject2D.Position"/> and <see cref="Texture"/> size.
        /// </summary>
        public Rectangle BoundingBox
        {
            get
            {
                if (Texture == null || Position == null)
                    throw new NullReferenceException($"Property {nameof(Texture)} or {nameof(Position)} is null!");

                return new Rectangle(Position.ToPoint(), Texture.Bounds.Size * Scale.ToPoint());
            }
        }

        protected GameSprite()
        {
            SpriteEffect = SpriteEffects.None;
            Color = Color.White;
        }

        public GameSprite(string assetName) : this()
        {
            _assetName = assetName;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            if (string.IsNullOrEmpty(_assetName))
                return;

            Texture = contentManager?.Load<Texture2D>(_assetName);

            base.LoadContent(contentManager);
        }

        public override void Draw(RenderContext renderContext)
        {
            if (IsVisible && renderContext != null)
            {
                renderContext.SpriteBatch.Draw(
                    Texture,
                    Position,
                    SourceRectangle,
                    Color,
                    MathHelper.ToRadians(Rotation),
                    Vector2.Zero,
                    Scale,
                    SpriteEffect,
                    LayerDepth);

                base.Draw(renderContext);
            }
        }
    }
}
