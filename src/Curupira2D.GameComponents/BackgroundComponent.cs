using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.GameComponents
{
    /// <summary>
    /// Draw simple infinity scroll scene.
    /// </summary>
    public class BackgroundComponent(Game game, string assetName, float velocity) : DrawableGameComponent(game)
    {
        Texture2D _bgdTexture;
        SpriteBatch _spriteBatch;
        private float _bgdPosX;

        protected override void LoadContent()
        {
            _spriteBatch = Game.Services.GetService<SpriteBatch>();

            if (_spriteBatch == null)
                _spriteBatch = new SpriteBatch(GraphicsDevice);

            if (_bgdTexture == null)
                _bgdTexture = Game.Content.Load<Texture2D>(assetName);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _bgdPosX -= (float)(velocity * gameTime.ElapsedGameTime.TotalSeconds);

            if (_bgdPosX <= -(GraphicsDevice.Viewport.Width))
                _bgdPosX = 0f;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.FrontToBack);

            _spriteBatch.Draw(_bgdTexture, new Rectangle(
                (int)_bgdPosX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            _spriteBatch.Draw(_bgdTexture, new Rectangle(
                (int)_bgdPosX + GraphicsDevice.Viewport.Width, 0, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
