using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Helper.Core
{
    /// <summary>
    /// Initialize property <see cref="RenderContext"/>.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        protected RenderContext RenderContext { get; private set; }

        protected override void Initialize()
        {
            RenderContext = new RenderContext
            {
                GraphicsDevice = GraphicsDevice,
                SpriteBatch = new SpriteBatch(GraphicsDevice)
            };

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            RenderContext.DeltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            RenderContext.GameTime = gameTime;

            base.Update(gameTime);
        }
    }
}
