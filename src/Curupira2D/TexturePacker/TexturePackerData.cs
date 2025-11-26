using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Curupira2D.TexturePacker
{
    public class TexturePackerData
    {
        [JsonPropertyName("frames")]
        public List<TextureAtlas> Frames { get; internal set; }
        [JsonPropertyName("meta")]
        public Meta Meta { get; internal set; }

        public TextureAtlas GetByName(string name) => Frames.Find(f => f.Filename == name);

        public List<TextureAtlas> GetWithRegex(string regexPattern) => Frames.FindAll(f => Regex.IsMatch(f.Filename, regexPattern));
    }

    public class Meta
    {
        [JsonPropertyName("app")]
        public string App { get; internal set; }
        [JsonPropertyName("version")]
        public string Version { get; internal set; }
        [JsonPropertyName("image")]
        public string Image { get; internal set; }
        [JsonPropertyName("format")]
        public string Format { get; internal set; }
        [JsonPropertyName("size")]
        public Size Size { get; internal set; }
        [JsonPropertyName("scale")]
        public int Scale { get; internal set; }
    }

    public class TextureAtlas
    {
        [JsonPropertyName("filename")]
        public string Filename { get; internal set; }
        [JsonPropertyName("frame")]
        public Source Frame { get; internal set; }
        [JsonPropertyName("rotated")]
        public bool Rotated { get; internal set; }
        [JsonPropertyName("trimmed")]
        public bool Trimmed { get; internal set; }
        [JsonPropertyName("spriteSourceSize")]
        public Source SpriteSourceSize { get; internal set; }
        [JsonPropertyName("sourceSize")]
        public Size SourceSize { get; internal set; }
        [JsonPropertyName("pivot")]
        public Pivot Pivot { get; internal set; }
    }

    public class Source
    {
        [JsonPropertyName("x")]
        public int X { get; internal set; }
        [JsonPropertyName("y")]
        public int Y { get; internal set; }
        [JsonPropertyName("w")]
        public int W { get; internal set; }
        [JsonPropertyName("h")]
        public int H { get; internal set; }

        public Rectangle ToRectangle() => new(X, Y, W, H);
    }

    public class Size
    {
        [JsonPropertyName("w")]
        public int W { get; internal set; }
        [JsonPropertyName("h")]
        public int H { get; internal set; }

        public Point ToPoint() => new(W, H);
    }

    public class Pivot
    {
        [JsonPropertyName("x")]
        public float X { get; internal set; }
        [JsonPropertyName("y")]
        public float Y { get; internal set; }

        public Vector2 ToVector2() => new(X, Y);
    }
}
