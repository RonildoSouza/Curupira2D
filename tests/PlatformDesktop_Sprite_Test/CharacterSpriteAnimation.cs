using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper;
using MonoGame.Helper.Core;
using System;

namespace PlatformDesktop_Sprite_Test
{
    class CharacterSpriteAnimation : GameObject2D
    {
        GameSpriteAnimation _spriteAnimation;

        public CharacterSpriteAnimation()
        {
            Velocity = new Vector2(100);
            _spriteAnimation = new GameSpriteAnimation("character", 4, 4, TimeSpan.FromMilliseconds(100), new Rectangle(0, 0, 60, 90));
        }

        public void MoveLeft(RenderContext renderContext)
            => HorizontalMove(renderContext, 180, 90, false);

        public void MoveUp(RenderContext renderContext)
            => VerticalMove(renderContext, 90, 90, false);

        public void MoveRight(RenderContext renderContext)
            => HorizontalMove(renderContext, 270, 90);

        public void MoveDown(RenderContext renderContext)
            => VerticalMove(renderContext, 0, 90);

        public override void LoadContent(ContentManager contentManager)
        {
            _spriteAnimation.LoadContent(contentManager);

            base.LoadContent(contentManager);
        }

        public override void Update(RenderContext renderContext)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Left))
                MoveLeft(renderContext);

            if (ks.IsKeyDown(Keys.Up))
                MoveUp(renderContext);

            if (ks.IsKeyDown(Keys.Right))
                MoveRight(renderContext);

            if (ks.IsKeyDown(Keys.Down))
                MoveDown(renderContext);

            //_spriteAnimation.AnimateAll(renderContext.GameTime);
            //_spriteAnimation.Pause();
            Position = _spriteAnimation.Position;

            base.Update(renderContext);
        }

        public override void Draw(RenderContext renderContext)
        {
            _spriteAnimation.Draw(renderContext);
            base.Draw(renderContext);
        }

        private void HorizontalMove(RenderContext renderContext, int sourcePosY, int sourceHeight, bool moveRight = true)
        {
            var tempPosition = _spriteAnimation.Position;
            var direction = moveRight ? 1 : -1;

            _spriteAnimation.Play();
            _spriteAnimation.AnimatePerRow(renderContext.GameTime, sourcePosY, sourceHeight);

            tempPosition.X += (float)(Velocity.X * renderContext.DeltaTime) * direction;
            _spriteAnimation.Position = tempPosition;
        }

        private void VerticalMove(RenderContext renderContext, int sourcePosY, int sourceHeight, bool moveDown = true)
        {
            var tempPosition = _spriteAnimation.Position;
            var direction = moveDown ? 1 : -1;

            _spriteAnimation.Play();
            _spriteAnimation.AnimatePerRow(renderContext.GameTime, sourcePosY, sourceHeight);

            tempPosition.Y += (float)(Velocity.Y * renderContext.DeltaTime) * direction;
            _spriteAnimation.Position = tempPosition;
        }
    }
}
