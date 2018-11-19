using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper;
using System;

namespace PlatformDesktop_Sprite_Test
{
    class CharacterSpriteAnimation : GameObject2D
    {
        GameSpriteAnimation _spriteAnimation;

        public CharacterSpriteAnimation(Game game) : base(game)
        {
            _spriteAnimation = new GameSpriteAnimation(game, "character", 4, 4, TimeSpan.FromMilliseconds(200), new Rectangle(0, 0, 60, 90));
        }

        public void MoveLeft()
            => HorizontalMove(180, 90, false);

        public void MoveUp()
            => VerticalMove(90, 90, false);

        public void MoveRight()
            => HorizontalMove(270, 90);

        public void MoveDown()
            => VerticalMove(0, 90);

        public override void LoadContent()
        {
            _spriteAnimation.LoadContent();
            base.LoadContent();
        }

        public override void Update()
        {
            base.Update();

            var ks = Keyboard.GetState();
            var deltaTime = RenderContext.GameTime.ElapsedGameTime.TotalSeconds;

            if (ks.IsKeyDown(Keys.Left))
                MoveLeft();

            if (ks.IsKeyDown(Keys.Up))
                MoveUp();

            if (ks.IsKeyDown(Keys.Right))
                MoveRight();

            if (ks.IsKeyDown(Keys.Down))
                MoveDown();

            //_spriteAnimation.AnimateAll(gameTime);
            _spriteAnimation.Pause();
        }

        public override void Draw()
        {
            _spriteAnimation.Draw();
            base.Draw();
        }

        private void HorizontalMove(int sourcePosY, int sourceHeight, bool moveRight = true)
        {
            var deltaTime = RenderContext.GameTime.ElapsedGameTime.TotalSeconds;
            var tempPosition = Position;
            var direction = moveRight ? 1 : -1;

            _spriteAnimation.Play();
            _spriteAnimation.AnimatePerRow(RenderContext.GameTime, sourcePosY, sourceHeight);
            tempPosition.X += (float)(Velocity.X * deltaTime) * direction;
            Position = tempPosition;
        }

        private void VerticalMove(int sourcePosY, int sourceHeight, bool moveDown = true)
        {
            var deltaTime = RenderContext.GameTime.ElapsedGameTime.TotalSeconds;
            var tempPosition = Position;
            var direction = moveDown ? 1 : -1;

            _spriteAnimation.Play();
            _spriteAnimation.AnimatePerRow(RenderContext.GameTime, sourcePosY, sourceHeight);
            tempPosition.Y += (float)(Velocity.Y * deltaTime) * direction;
            Position = tempPosition;
        }
    }
}
