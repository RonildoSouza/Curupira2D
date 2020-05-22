using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper;
using MonoGame.Helper.Core;

namespace GamePixelRPG.GameObjects.Weapons
{
    public class Sword : GameSprite
    {
        public Sword(RenderContext renderContext)
        {
            Texture = new Texture2D(renderContext.GraphicsDevice, 30, 150);
            //Texture.SetData(new Color[] { Color.DarkBlue });
        }
    }
}