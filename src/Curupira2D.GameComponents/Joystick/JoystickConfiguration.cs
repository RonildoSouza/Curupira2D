using Microsoft.Xna.Framework;

namespace Curupira2D.GameComponents.Joystick
{
    public class JoystickConfiguration
    {
        public JoystickConfiguration(int size, Vector2 position)
        {
            Size = size;
            Position = position;
            JoystickHandleSize = JoystickHandleSize.Medium;
        }

        public int Size { get; }
        public Vector2 Position { get; }
        public JoystickHandleSize JoystickHandleSize { get; set; }
        public bool InvertX_Axis { get; set; }
        public bool InvertY_Axis { get; set; }
    }
}
