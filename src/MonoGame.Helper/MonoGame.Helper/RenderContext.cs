using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Helper
{
    public sealed class RenderContext : GameComponent
    {
        SpriteBatch _spriteBatch;
        GraphicsDevice _graphicsDevice;
        ContentManager _contentManager;

        public SpriteBatch SpriteBatch
        {
            get
            {
                _spriteBatch = Game.Services.GetService<SpriteBatch>();

                if (_spriteBatch == null)
                    _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

                return _spriteBatch;
            }
        }
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                if (_graphicsDevice == null)
                    _graphicsDevice = Game.GraphicsDevice;

                return _graphicsDevice;
            }
        }
        public ContentManager ContentManager
        {
            get
            {
                if (_contentManager == null)
                    _contentManager = Game.Content;

                return _contentManager;
            }
        }
        public GameTime GameTime { get; private set; }

        public RenderContext(Game game) : base(game) { }

        public override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            base.Update(gameTime);
        }
    }
}
