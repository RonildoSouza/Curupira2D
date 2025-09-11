using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledLib;

namespace Curupira2D.ECS.Systems.Drawables
{
    internal class TileInfo(Tile tile, Point position)
    {
        SpriteEffects _spriteEffect;
        float _rotation;

        public Tile Tile
        {
            get => tile;
            set
            {
                tile = value;
                (_spriteEffect, _rotation) = GetTileOrientation(tile);
            }
        }

        public SpriteEffects SpriteEffect => _spriteEffect;
        public float Rotation => _rotation;
        public Point Position => position;

        static (SpriteEffects TileSpriteEffect, float TileRotation) GetTileOrientation(Tile tile)
        {
            var tileSpriteEffect = SpriteEffects.None;
            var tileRotation = 0f;

            switch (tile.Orientation)
            {
                case TileOrientation.FlippedH:
                    tileSpriteEffect = SpriteEffects.FlipHorizontally;
                    break;
                case TileOrientation.FlippedV:
                    tileSpriteEffect = SpriteEffects.FlipHorizontally;
                    tileRotation = 180f;
                    break;
                case TileOrientation.FlippedAD:
                    tileSpriteEffect = SpriteEffects.FlipHorizontally;
                    tileRotation = -90f;
                    break;
                case TileOrientation.Rotate90CW:
                    tileRotation = 90f;
                    break;
                case TileOrientation.Rotate180CCW:
                    tileSpriteEffect = SpriteEffects.FlipHorizontally;
                    tileRotation = -270f;
                    break;
                case TileOrientation.Rotate270CCW:
                    tileSpriteEffect = SpriteEffects.FlipVertically;
                    tileRotation = 270f;
                    break;
            }

            return (tileSpriteEffect, tileRotation);
        }
    }
}
