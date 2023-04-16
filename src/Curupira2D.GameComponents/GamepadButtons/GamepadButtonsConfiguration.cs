using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.GameComponents.GamepadButtons
{
    public class GamepadButtonsConfiguration
    {
        public GamepadButtonsConfiguration(
            int size,
            Vector2 position,
            Texture2D texture)
        {
            Size = size;
            Position = position;
            Texture = texture;
            Opacity = 0.6f;
            Color = Color.White;
        }

        public int Size { get; }
        public Vector2 Position { get; }
        public Texture2D Texture { get; }
        public float Opacity { get; set; }
        public Color Color { get; set; }
    }
}