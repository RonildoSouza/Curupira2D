using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace MonoGame.Helper.Extensions
{
    public static class GraphicsDeviceGeometricPrimitivesExtension
    {
        public static Texture2D CreateTextureRectangle(this GraphicsDevice graphicsDevice, Point size, Color color)
        {
            var colorData = Enumerable.Range(0, size.X * size.Y)
                .Select(_ => color)
                .ToArray();

            var texture = new Texture2D(graphicsDevice, size.X, size.Y);
            texture.SetData(colorData);

            return texture;
        }

        public static Texture2D CreateTextureRectangle(this GraphicsDevice graphicsDevice, Vector2 size, Color color)
            => CreateTextureRectangle(graphicsDevice, size.ToPoint(), color);

        public static Texture2D CreateTextureRectangle(this GraphicsDevice graphicsDevice, int width, int height, Color color)
            => CreateTextureRectangle(graphicsDevice, new Point(width, height), color);

        public static Texture2D CreateTextureRectangle(this GraphicsDevice graphicsDevice, float width, float height, Color color)
            => CreateTextureRectangle(graphicsDevice, new Point((int)width, (int)height), color);

        public static Texture2D CreateTextureRectangle(this GraphicsDevice graphicsDevice, int size, Color color)
           => CreateTextureRectangle(graphicsDevice, new Point(size), color);

        public static Texture2D CreateTextureCircle(this GraphicsDevice graphicsDevice, int radius, Color color, int tickenes = 0)
        {
            var diameter = 2 * radius;
            var texture = new Texture2D(graphicsDevice, diameter, diameter);
            var colorData = new Color[diameter * diameter];
            var radiusSQ = radius * radius;

            if (tickenes >= radius)
                tickenes = radius - 5;

            var intDiam = (radius - tickenes) / 2f;
            var intDiamSQ = intDiam * intDiam;

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    var i = x * diameter + y;
                    var pos = new Vector2(x - radius, y - radius);

                    if (pos.LengthSquared() <= radiusSQ)
                        colorData[i] = color;
                    else
                        colorData[i] = Color.Transparent;

                    if (tickenes != 0 && pos.LengthSquared() <= intDiamSQ)
                        colorData[i] = Color.Transparent;
                }
            }

            texture.SetData(colorData);
            return texture;
        }

        public static Texture2D CreateTextureCircle(this GraphicsDevice graphicsDevice, float radius, Color color, int tickenes = 0)
            => CreateTextureCircle(graphicsDevice, (int)radius * 2, color, tickenes);
    }
}
