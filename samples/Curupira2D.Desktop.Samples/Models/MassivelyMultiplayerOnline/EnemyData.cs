using Microsoft.Xna.Framework;

namespace Curupira2D.Desktop.Samples.Models.MassivelyMultiplayerOnline
{
    internal class EnemyData
    {
        public EnemyData(EnemyDataType type, string uniqueId, Vector2 position, Color color, Point textureSize, (Point, Point) eyesTextureSize)
        {
            Type = type;
            UniqueId = uniqueId;
            Position = position;
            Color = color;
            TextureSize = textureSize;
            EyesTextureSize = eyesTextureSize;
        }

        public EnemyDataType Type { get; }
        public string UniqueId { get; }
        public Vector2 Position { get; }
        public Color Color { get; }
        public Point TextureSize { get; }
        public (Point, Point) EyesTextureSize { get; }
    }

    internal enum EnemyDataType
    {
        Joined,
        Left,
        Message,
    }
}
