using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Helper.Components
{
    public class FPSComponent : DrawableGameComponent
    {
        SpriteBatch _spriteBatch;
        SpriteFont _spriteFont;
        string _fontAssetName;
        TimeSpan _elapsedTime;
        int _totalFrames;
        int _fps;

        public Color Color { get; set; }
        public Vector2 Position { get; set; }

        public FPSComponent(Game game, string fontAssetName) : base(game)
        {
            _fontAssetName = fontAssetName;
        }

        public FPSComponent(Game game, string fontName, Vector2 position) : this(game, fontName)
        {
            Position = position; ;
        }

        public FPSComponent(Game game, string fontName, Vector2 position, Color color) : this(game, fontName, position)
        {
            Color = color;
        }

        public override void Initialize()
        {
            Color = Color.White;
            Position = Vector2.Zero;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = Game.Services.GetService<SpriteBatch>();

            if (_spriteBatch == null)
                _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            if (!string.IsNullOrEmpty(_fontAssetName.Trim()) && _spriteFont == null)
                _spriteFont = Game.Content.Load<SpriteFont>(_fontAssetName);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            _totalFrames++;

            if (_elapsedTime >= TimeSpan.FromSeconds(1))
            {
                _fps = _totalFrames;
                _totalFrames = 0;
                _elapsedTime -= TimeSpan.FromSeconds(1);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            _spriteBatch.DrawString(_spriteFont, $"FPS --> {_fps}", Position, Color);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
