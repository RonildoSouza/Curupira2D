using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper;
using MonoGame.Helper.Core;

namespace GamePixelRPG.GameObjects.Characters
{
    public class Human : GameSprite
    {
        public Human(RenderContext renderContext)
        {
            Texture = new Texture2D(renderContext.GraphicsDevice, 100, 300, false, SurfaceFormat.Alpha8);
            //Texture.SetData(new Color[] { Color.DarkRed });
        }
    }
}