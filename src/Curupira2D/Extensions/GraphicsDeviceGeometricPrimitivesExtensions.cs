using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Curupira2D.Extensions
{
    public static class GraphicsDeviceGeometricPrimitivesExtensions
    {
        #region CreateTextureRectangle Methods
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
            => graphicsDevice.CreateTextureRectangle(size.ToPoint(), color);

        public static Texture2D CreateTextureRectangle(this GraphicsDevice graphicsDevice, int width, int height, Color color)
            => graphicsDevice.CreateTextureRectangle(new Point(width, height), color);

        public static Texture2D CreateTextureRectangle(this GraphicsDevice graphicsDevice, float width, float height, Color color)
            => graphicsDevice.CreateTextureRectangle(new Point((int)width, (int)height), color);

        public static Texture2D CreateTextureRectangle(this GraphicsDevice graphicsDevice, int size, Color color)
           => graphicsDevice.CreateTextureRectangle(new Point(size), color);
        #endregion

        #region CreateTextureCircle Methods
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
            => graphicsDevice.CreateTextureCircle((int)radius * 2, color, tickenes);
        #endregion
    }
}
