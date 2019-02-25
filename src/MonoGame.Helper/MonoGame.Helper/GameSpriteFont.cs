using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.Core;

namespace MonoGame.Helper
{
    public class GameSpriteFont : GameSprite
    {
        public SpriteFont SpriteFont { get; set; }
        public string Text { get; set; }

        public GameSpriteFont(string assetName, string text) : base()
        {
            _assetName = assetName;
            Text = text;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            if (string.IsNullOrEmpty(_assetName))
                return;

            SpriteFont = contentManager?.Load<SpriteFont>(_assetName);
        }

        public override void Draw(RenderContext renderContext)
        {
            if (IsVisible && renderContext != null)
            {
                renderContext.SpriteBatch.DrawString(
                    SpriteFont,
                    Text,
                    Position,
                    Color,
                    MathHelper.ToRadians(Rotation),
                    Vector2.Zero,
                    Scale,
                    SpriteEffect,
                    LayerDepth);
            }
        }
    }
}
