using Curupira2D.ECS.Components.Drawables;
using Curupira2D.TexturePacker;
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
                ArgumentNullException.ThrowIfNull($"Argument {nameof(content)} or {nameof(tiledMapStream)} can't be null!");

            tiledMapStream = FixStreamPosition(tiledMapStream);
            return Map.FromStream(tiledMapStream);
        }

        public static Map LoadTiledMap(this ContentManager content, string tiledMapRelativePath)
        {
            if (string.IsNullOrEmpty(tiledMapRelativePath))
                ArgumentNullException.ThrowIfNull($"Argument {nameof(tiledMapRelativePath)} can't be null or empty!");

            var tiledMapExtension = Path.GetExtension(tiledMapRelativePath);
            if (tiledMapExtension != ".tmx" && tiledMapExtension != ".json" && tiledMapExtension != ".tmj")
                throw new FormatException("The Tiled Map must have the extension .tmx, .json or .tmj!");

            return LoadTiledMap(content, TitleContainer.OpenStream(Path.Combine(content.RootDirectory, tiledMapRelativePath)));
        }

        public static TiledMapComponent CreateTiledMapComponent(this ContentManager content, string tiledMapRelativePath, string tilesetRelativePath = null, Color color = default, bool fixedPosition = false)
        {
            var map = content.LoadTiledMap(tiledMapRelativePath);

            tilesetRelativePath ??= Regex.Replace(tiledMapRelativePath, @"\.tmx|\.json|\.tmj", string.Empty, RegexOptions.None, TimeSpan.FromSeconds(30));
            var tilesetTexture = content.Load<Texture2D>(tilesetRelativePath);

            return new TiledMapComponent(map, tilesetTexture, color, fixedPosition);
        }

        public static TiledMapComponent CreateTiledMapComponent(this ContentManager content, Stream tiledMapStream, string tilesetRelativePath, Color color = default, bool fixedPosition = false)
        {
            var map = content.LoadTiledMap(tiledMapStream);
            var tilesetTexture = content.Load<Texture2D>(tilesetRelativePath);

            return new TiledMapComponent(map, tilesetTexture, color, fixedPosition);
        }

        public static TexturePackerData LoadTexturePackerData(this ContentManager content, Stream texturePackerFileStream)
        {
            if (content == null || texturePackerFileStream == null)
                ArgumentNullException.ThrowIfNull($"Argument {nameof(content)} or {nameof(texturePackerFileStream)} can't be null!");

            return TexturePackerFileReader.Read(texturePackerFileStream);
        }

        public static TexturePackerData LoadTexturePackerData(this ContentManager content, string texturePackerFileRelativePath)
        {
            if (string.IsNullOrEmpty(texturePackerFileRelativePath))
                ArgumentNullException.ThrowIfNull($"Argument {nameof(texturePackerFileRelativePath)} can't be null or empty!");

            if (content == null)
                ArgumentNullException.ThrowIfNull($"Argument {nameof(content)} can't be null!");

            var texturePackerFileExtension = Path.GetExtension(texturePackerFileRelativePath);
            if (texturePackerFileExtension != ".json")
                throw new FormatException("The Texture Packer File must have the extension .json!");

            return TexturePackerFileReader.Read(Path.Combine(content.RootDirectory, texturePackerFileRelativePath));
        }

        private static Stream FixStreamPosition(Stream stream)
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
