using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Helper.Core
{
    public sealed class RenderContext
    {
        public double DeltaTime { get; internal set; }
        public GameTime GameTime { get; internal set; }
        public GraphicsDevice GraphicsDevice { get; internal set; }
        public SpriteBatch SpriteBatch { get; internal set; }
    }
}