using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace PlatformDesktop_Sprite_Test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : MonoGame.Helper.Core.Game
    {
        GraphicsDeviceManager graphics;

        CharacterSpriteAnimation _characterSprite;
        SpriteFont _spriteFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _characterSprite = new CharacterSpriteAnimation();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            _characterSprite.LoadContent(Content);
            _spriteFont = Content.Load<SpriteFont>("font");
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _characterSprite.Update(RenderContext);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here           
            var sb = new StringBuilder();
            sb.AppendLine($"Position: {_characterSprite.Position}");

            RenderContext.SpriteBatch.Begin();

            RenderContext.SpriteBatch.DrawString(_spriteFont, sb, Vector2.Zero, Color.Black);
            _characterSprite.Draw(RenderContext);

            RenderContext.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
