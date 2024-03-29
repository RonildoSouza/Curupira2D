﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Curupira2D.GameComponents
{
    public class LineGridComponent : DrawableGameComponent, IEquatable<LineGridComponent>
    {
        SpriteBatch _spriteBatch;
        Texture2D _texture;

        public Vector2 Size { get; set; }
        public Color Color { get; set; }

        public LineGridComponent(Game game, Vector2 size, Color color) : base(game)
        {
            Size = size;
            Color = color;

            Load(game);
        }

        public override void Draw(GameTime gameTime)
        {
            var cols = (int)Math.Round(GraphicsDevice.Viewport.Width / (Size.X - 2));
            var rows = (int)Math.Round(GraphicsDevice.Viewport.Height / (Size.Y - 2));

            _spriteBatch.Begin(SpriteSortMode.BackToFront);

            for (float x = -cols; x < cols; x++)
            {
                var rectangle = new Rectangle((int)(x * Size.X), 0, 1, GraphicsDevice.Viewport.Height);
                _spriteBatch.Draw(_texture, rectangle, Color);
            }

            for (float y = -rows; y < rows; y++)
            {
                var rectangle = new Rectangle(0, (int)(y * Size.Y), GraphicsDevice.Viewport.Width, 1);
                _spriteBatch.Draw(_texture, rectangle, Color);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void Load(Game game)
        {
            _spriteBatch = game.Services.GetService<SpriteBatch>();

            if (_spriteBatch == null)
                _spriteBatch = new SpriteBatch(GraphicsDevice);

            _texture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _texture.SetData(new[] { Color.White });
        }

        public bool Equals(LineGridComponent other)
            => other != null && other.Size == Size && other.Color == Color;
    }
}
