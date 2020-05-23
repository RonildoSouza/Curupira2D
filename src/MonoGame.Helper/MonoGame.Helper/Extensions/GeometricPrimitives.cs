using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace MonoGame.ECS.Helpers
{
    public static class GeometricPrimitives
    {
        public static Texture2D CreateSquare(GraphicsDevice graphicsDevice, Point size, Color color)
        {
            var colorData = Enumerable.Range(0, size.X * size.Y)
                .Select(_ => color)
                .ToArray();

            var texture = new Texture2D(graphicsDevice, size.X, size.Y);
            texture.SetData(colorData);

            return texture;
        }

        public static Texture2D CreateSquare(GraphicsDevice graphicsDevice, Vector2 size, Color color)
            => CreateSquare(graphicsDevice, size.ToPoint(), color);

        public static Texture2D CreateSquare(GraphicsDevice graphicsDevice, int width, int height, Color color)
            => CreateSquare(graphicsDevice, new Point(width, height), color);

        public static Texture2D CreateSquare(GraphicsDevice graphicsDevice, float width, float height, Color color)
            => CreateSquare(graphicsDevice, new Point((int)width, (int)height), color);

        public static Texture2D CreateSquare(GraphicsDevice graphicsDevice, int size, Color color)
           => CreateSquare(graphicsDevice, new Point(size), color);

        public static Texture2D CreateCircle(GraphicsDevice graphicsDevice, int radius, Color color, int tickenes = 0)
        {
            var texture = new Texture2D(graphicsDevice, radius, radius);
            var colorData = new Color[radius * radius];
            var diam = radius / 2f;
            var diamsq = diam * diam;

            if (tickenes >= radius)
                tickenes = radius - 5;

            var intdiam = (radius - tickenes) / 2f;
            var intdiamsq = intdiam * intdiam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    var index = x * radius + y;
                    var pos = new Vector2(x - diam, y - diam);

                    if (pos.LengthSquared() <= diamsq)
                        colorData[index] = color;
                    else
                        colorData[index] = Color.Transparent;

                    if (tickenes != 0 && pos.LengthSquared() <= intdiamsq)
                        colorData[index] = Color.Transparent;
                }
            }

            texture.SetData(colorData);
            return texture;
        }

        public static Texture2D CreateCircle(GraphicsDevice graphicsDevice, float radius, Color color, int tickenes = 0)
            => CreateCircle(graphicsDevice, (int)radius, color, tickenes);
    }
}
