using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.GameComponents.Joystick
{
    public class JoystickTexture
    {
        public JoystickTexture(Texture2D texture, float opacity = 0.6f)
        {
            Texture = texture;
            Opacity = opacity;
        }

        public Texture2D Texture { get; private set; }
        public float Opacity { get; private set; }
    }
}
