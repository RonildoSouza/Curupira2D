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
        public static Map LoadTiledMap(this ContentManager content, string tiledMapRelativePath)
        {
            if (content == null || string.IsNullOrEmpty(tiledMapRelativePath))
                throw new ArgumentNullException($"Argument {nameof(content)} or {nameof(tiledMapRelativePath)} can't be null or empty!");

            var tiledMapExtension = Path.GetExtension(tiledMapRelativePath);
            if (tiledMapExtension != ".tmx" && tiledMapExtension != ".json")
                throw new FormatException("The Tiled Map must have the extension .tmx or .json!");

            var tiledMapFilePath = Path.Combine(content.RootDirectory, tiledMapRelativePath);

            return Map.FromStream(File.OpenRead(tiledMapFilePath));
        }

        public static TiledMapComponent CreateTiledMapComponent(this ContentManager content, string tiledMapRelativePath, string tilesetRelativePath = null, Color color = default, bool fixedPosition = false)
        {
            var map = content.LoadTiledMap(tiledMapRelativePath);

            tilesetRelativePath ??= Regex.Replace(tiledMapRelativePath, @"\.tmx|\.json", string.Empty);
            var tilesetTexture = content.Load<Texture2D>(tilesetRelativePath);

            return new TiledMapComponent(map, tilesetTexture, color, fixedPosition);
        }
    }
}
