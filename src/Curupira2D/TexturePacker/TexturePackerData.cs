using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Curupira2D.TexturePacker
{
    public class TexturePackerData
    {
        [JsonPropertyName("frames")]
        public List<TextureAtlas> Frames { get; set; }
        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }

        public TextureAtlas GetByName(string name) => Frames.Find(f => f.Filename == name);

        public List<TextureAtlas> GetWithRegex(string regexPattern) => Frames.FindAll(f => Regex.IsMatch(f.Filename, regexPattern));
    }

    public class Meta
    {
        [JsonPropertyName("app")]
        public string App { get; set; }
        [JsonPropertyName("version")]
        public string Version { get; set; }
        [JsonPropertyName("image")]
        public string Image { get; set; }
        [JsonPropertyName("format")]
        public string Format { get; set; }
        [JsonPropertyName("size")]
        public Size Size { get; set; }
        [JsonPropertyName("scale")]
        public int Scale { get; set; }
    }

    public class TextureAtlas
    {
        [JsonPropertyName("filename")]
        public string Filename { get; set; }
        [JsonPropertyName("frame")]
        public Source Frame { get; set; }
        [JsonPropertyName("rotated")]
        public bool Rotated { get; set; }
        [JsonPropertyName("trimmed")]
        public bool Trimmed { get; set; }
        [JsonPropertyName("spriteSourceSize")]
        public Source SpriteSourceSize { get; set; }
        [JsonPropertyName("sourceSize")]
        public Size SourceSize { get; set; }
        [JsonPropertyName("pivot")]
        public Pivot Pivot { get; set; }
    }

    public class Source
    {
        [JsonPropertyName("x")]
        public int X { get; set; }
        [JsonPropertyName("y")]
        public int Y { get; set; }
        [JsonPropertyName("w")]
        public int W { get; set; }
        [JsonPropertyName("h")]
        public int H { get; set; }

        public Rectangle ToRectangle() => new(X, Y, W, H);
    }

    public class Size
    {
        [JsonPropertyName("w")]
        public int W { get; set; }
        [JsonPropertyName("h")]
        public int H { get; set; }

        public Point ToPoint() => new(W, H);
    }

    public class Pivot
    {
        [JsonPropertyName("x")]
        public float X { get; set; }
        [JsonPropertyName("y")]
        public float Y { get; set; }

        public Vector2 ToVector2() => new(X, Y);
    }
}
