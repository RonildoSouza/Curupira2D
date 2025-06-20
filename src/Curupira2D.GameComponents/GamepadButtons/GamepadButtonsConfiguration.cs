using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.GameComponents.GamepadButtons
{
    public class GamepadButtonsConfiguration(int size, Vector2 position, Texture2D texture)
    {
        public int Size { get; } = size;
        public Vector2 Position { get; } = position;
        public Texture2D Texture { get; } = texture;
        public float Opacity { get; set; } = 0.6f;
        public Color Color { get; set; } = Color.White;
    }
}