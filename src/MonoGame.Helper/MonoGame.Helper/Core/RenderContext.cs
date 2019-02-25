using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Helper.Core
{
    /// <summary>
    /// Contains common properties to renderization.
    /// </summary>
    public sealed class RenderContext
    {
        /// <summary>
        /// Elapsed game time in seconds.
        /// </summary>
        public double DeltaTime { get; internal set; }
        public GameTime GameTime { get; internal set; }
        public GraphicsDevice GraphicsDevice { get; internal set; }
        public SpriteBatch SpriteBatch { get; internal set; }
    }
}