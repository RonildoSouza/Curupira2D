using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public GameSprite(Game game, string assetName) : base(game)
        {
            _assetName = assetName;
            SpriteEffect = SpriteEffects.None;
            Color = Color.White;
        }

        public override void LoadContent()
        {
            if (string.IsNullOrEmpty(_assetName) || RenderContext == null)
                return;

            Texture = RenderContext.ContentManager?.Load<Texture2D>(_assetName);
            base.LoadContent();
        }

        public override void Draw()
        {
            if (IsVisible && RenderContext != null)
            {
                RenderContext.SpriteBatch?.Draw(
                    Texture,
                    Position,
                    SourceRectangle,
                    Color,
                    MathHelper.ToRadians(Rotation),
                    Vector2.Zero,
                    Scale,
                    SpriteEffect,
                    LayerDepth);

                Position.Normalize();
                base.Draw();
            }
        }
    }
}
