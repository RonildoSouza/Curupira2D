using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.Utilities;
using System;

namespace Curupira2D.Diagnostics
{
    public class DebugComponent : DrawableGameComponent, IEquatable<DebugComponent>
    {
        GameCore _gameCore;
        SpriteFont _fontArial18;
        SpriteBatch _spriteBatch;
        string _text;
        Vector2 _textSize;

        public DebugComponent(GameCore game) : base(game)
        {
            _gameCore = game;
        }

        protected override void LoadContent()
        {
            _spriteBatch = _gameCore.GetCurrentScene()?.SpriteBatch == null
                ? new SpriteBatch(GraphicsDevice)
                : _gameCore.GetCurrentScene()?.SpriteBatch;

            _fontArial18 = _gameCore.Content.Load<SpriteFont>("FontArial18");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (_gameCore.DebugOptions.DebugActive)
            {
                var title = !string.IsNullOrEmpty(_gameCore.GetCurrentScene()?.Title) ? _gameCore.GetCurrentScene().Title : GetType().Assembly.GetName().Name;
                _gameCore.Window.Title = $"{title}" +
                           $" | {GraphicsDevice.Viewport.Width}x{GraphicsDevice.Viewport.Height}" +
                           $" | FPS: {_gameCore.FPS}" +
                           $" | v{_gameCore.GetVersion()}";

                _text = _gameCore.Window.Title;
                _textSize = _fontArial18.MeasureString(_text);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_gameCore.DebugOptions.DebugActive)
            {
                switch (PlatformInfo.MonoGamePlatform)
                {
                    case MonoGamePlatform.Android:
                        _spriteBatch.Begin();
                        _spriteBatch.DrawString(_fontArial18, _text, new Vector2((GraphicsDevice.Viewport.Width * 0.5f) - (_textSize.X * 0.5f), _textSize.Y * 1.1f), _gameCore.DebugOptions.TextColor);
                        _spriteBatch.End();
                        break;
                    default:
                        break;
                }
            }

            base.Draw(gameTime);
        }

        public bool Equals(DebugComponent other) => other != null;
    }
}
