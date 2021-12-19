using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.GameComponents.Joystick
{
    public class JoystickConfiguration
    {
        public JoystickConfiguration(int size, Vector2 position)
        {
            Size = size;
            Position = position;
            HandleSize = JoystickHandleSize.Medium;
            BackgroundOpacity = 0.6f;
            HandleOpacity = 0.6f;
        }

        public int Size { get; }
        public Vector2 Position { get; }
        public JoystickHandleSize HandleSize { get; set; }
        public Texture2D BackgroundTexture { get; set; }
        public float BackgroundOpacity { get; set; }
        public Texture2D HandleTexture { get; set; }
        public float HandleOpacity { get; set; }
        public bool InvertX_Axis { get; set; }
        public bool InvertY_Axis { get; set; }
    }
}
