using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.Utilities;
using System;

namespace Curupira2D.Diagnostics
{
    public class DebugComponent(GameCore gameCore) : DrawableGameComponent(gameCore), IEquatable<DebugComponent>
    {
        SpriteFont _diagnosticsFont;
        SpriteBatch _spriteBatch;
        string _text;
        Vector2 _textSize;
        TimeSpan _elapsedTime;
        int _totalFrames;
        float _totalMemory;

        protected override void LoadContent()
        {
            _spriteBatch = (gameCore.GetCurrentScene()?.SpriteBatch) ?? new SpriteBatch(GraphicsDevice);
            _diagnosticsFont = gameCore.Content.Load<SpriteFont>("DiagnosticsFont");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (gameCore.DebugOptions.DebugActive)
            {
                _totalFrames++;
                _elapsedTime += gameTime.ElapsedGameTime;

                if (_elapsedTime >= TimeSpan.FromSeconds(1))
                {
                    _totalMemory = GC.GetTotalMemory(false) / 1048576f;

                    var title = !string.IsNullOrEmpty(gameCore.GetCurrentScene()?.Title) ? gameCore.GetCurrentScene().Title : GetType().Assembly.GetName().Name;
                    gameCore.Window.Title = $"{title} v{gameCore.GetVersion()}" +
                               $"\r\n        {GraphicsDevice.Viewport.Width}x{GraphicsDevice.Viewport.Height}" +
                               $" | FPS: {_totalFrames}" +
                               $" | {_totalMemory:F} MB";

                    _text = gameCore.Window.Title;
                    _textSize = _diagnosticsFont.MeasureString(_text);
                    _totalFrames = 0;
                    _elapsedTime = TimeSpan.Zero;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (gameCore.DebugOptions.DebugActive)
            {
                switch (PlatformInfo.MonoGamePlatform)
                {
                    case MonoGamePlatform.Android:
                        _spriteBatch.Begin();
                        _spriteBatch.DrawString(_diagnosticsFont, _text ?? string.Empty, new Vector2((GraphicsDevice.Viewport.Width * 0.5f) - (_textSize.X * 0.5f), _textSize.Y * 1.1f), gameCore.DebugOptions.TextColor);
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
