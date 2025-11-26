using Microsoft.Xna.Framework;
using System.IO;
using System.Text.Json;

namespace Curupira2D.TexturePacker
{
    internal static class TexturePackerFileReader
    {
        internal static TexturePackerData Read(string path)
        {
            //using var stream = File.OpenRead(path);
            using var stream = TitleContainer.OpenStream(path);
            return Read(stream);
        }

        internal static TexturePackerData Read(Stream stream)
            => JsonSerializer.Deserialize<TexturePackerData>(stream);
    }
}
