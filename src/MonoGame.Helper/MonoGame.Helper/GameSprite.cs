using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.Core;

namespace MonoGame.Helper
{
    public class GameSprite : GameObject2D
    {
        readonly string _assetName;

        public Texture2D Texture { get; private set; }

        protected SpriteEffects SpriteEffect { get; set; }

        protected Color Color { get; set; }

        protected Rectangle? SourceRectangle { get; set; }

        protected float LayerDepth { get; set; }

        public GameSprite(string assetName)
        {
            _assetName = assetName;
            SpriteEffect = SpriteEffects.None;
            Color = Color.White;
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
