using Curupira2D.ECS.Components.Drawables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Text.RegularExpressions;
using TiledLib;

namespace Curupira2D.Extensions
{
    public static class ContentManagerExtensions
    {
        public static Map LoadTiledMap(this ContentManager content, Stream tiledMapStream)
        {
            if (content == null || tiledMapStream == null)
                throw new ArgumentNullException($"Argument {nameof(content)} or {nameof(tiledMapStream)} can't be null!");

            tiledMapStream = FixStreamPosition(tiledMapStream);
            return Map.FromStream(tiledMapStream);
        }

        public static Map LoadTiledMap(this ContentManager content, string tiledMapRelativePath)
        {
            if (string.IsNullOrEmpty(tiledMapRelativePath))
                throw new ArgumentNullException($"Argument {nameof(tiledMapRelativePath)} can't be null or empty!");

            var tiledMapExtension = Path.GetExtension(tiledMapRelativePath);
            if (tiledMapExtension != ".tmx" && tiledMapExtension != ".json" && tiledMapExtension != ".tmj")
                throw new FormatException("The Tiled Map must have the extension .tmx or .json!");

            var tiledMapFilePath = Path.Combine(content.RootDirectory, tiledMapRelativePath);

            return LoadTiledMap(content, TitleContainer.OpenStream(tiledMapFilePath));
        }

        public static TiledMapComponent CreateTiledMapComponent(this ContentManager content, string tiledMapRelativePath, string tilesetRelativePath = null, Color color = default, bool fixedPosition = false)
        {
            var map = content.LoadTiledMap(tiledMapRelativePath);

            tilesetRelativePath ??= Regex.Replace(tiledMapRelativePath, @"\.tmx|\.json|\.tmj", string.Empty);
            var tilesetTexture = content.Load<Texture2D>(tilesetRelativePath);

            return new TiledMapComponent(map, tilesetTexture, color, fixedPosition);
        }

        public static TiledMapComponent CreateTiledMapComponent(this ContentManager content, Stream tiledMapStream, string tilesetRelativePath, Color color = default, bool fixedPosition = false)
        {
            var map = content.LoadTiledMap(tiledMapStream);
            var tilesetTexture = content.Load<Texture2D>(tilesetRelativePath);

            return new TiledMapComponent(map, tilesetTexture, color, fixedPosition);
        }

        static Stream FixStreamPosition(Stream stream)
        {
            try
            {
                if (stream.Position > 0)
                    stream.Position = 0;

                return stream;
            }
            catch (NotSupportedException)
            {
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);

                if (memoryStream.Position > 0)
                    memoryStream.Position = 0;

                return memoryStream;
            }
        }
    }
}
