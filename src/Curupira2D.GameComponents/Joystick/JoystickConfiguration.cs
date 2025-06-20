using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.GameComponents.Joystick
{
    public class JoystickConfiguration(int size, Vector2 position)
    {
        public int Size { get; } = size;
        public Vector2 Position { get; } = position;
        public JoystickHandleSize HandleSize { get; set; } = JoystickHandleSize.Medium;
        public Texture2D BackgroundTexture { get; set; }
        public float BackgroundOpacity { get; set; } = 0.6f;
        public Texture2D HandleTexture { get; set; }
        public float HandleOpacity { get; set; } = 0.6f;
        public bool InvertX_Axis { get; set; }
        public bool InvertY_Axis { get; set; }
        public Point JoystickHandleMovimentScale { get; set; } = new Point(2);
        public Color Color { get; set; } = Color.White;
    }
}
