using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;

namespace SpriteAnimation
{
    [RequiredComponent(typeof(SpriteAnimationComponent))]
    public class CharacterMovimentSystem : MonoGame.Helper.ECS.System, IUpdatable
    {
        const float VELOCITY = 100f;

        public void Update()
        {
            var characterEntity = Scene.GetEntity("character");

            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Left))
                HorizontalMove(ref characterEntity, 180, false);

            if (ks.IsKeyDown(Keys.Up))
                VerticalMove(ref characterEntity, 90, false);

            if (ks.IsKeyDown(Keys.Right))
                HorizontalMove(ref characterEntity, 270);

            if (ks.IsKeyDown(Keys.Down))
                VerticalMove(ref characterEntity, 0);
        }

        void HorizontalMove(ref Entity characterEntity, int sourcePosY, bool moveRight = true)
        {
            var spriteAnimationComponent = characterEntity.GetComponent<SpriteAnimationComponent>();
            var tempPosition = characterEntity.Transform.Position;
            var direction = moveRight ? 1 : -1;

            var sourceRectangle = spriteAnimationComponent.SourceRectangle.Value;
            sourceRectangle.Y = sourcePosY;

            spriteAnimationComponent.IsPlaying = true;
            spriteAnimationComponent.SourceRectangle = sourceRectangle;

            tempPosition.X += (float)(VELOCITY * Scene.DeltaTime) * direction;

            #region Out of screen in left or right
            if (tempPosition.X + spriteAnimationComponent.FrameWidth < 0f)
                tempPosition.X = Scene.ScreenWidth;

            if (tempPosition.X > Scene.ScreenWidth)
                tempPosition.X = -spriteAnimationComponent.FrameWidth;
            #endregion

            characterEntity.SetPosition(tempPosition);
        }

        void VerticalMove(ref Entity characterEntity, int sourcePosY, bool moveDown = true)
        {
            var spriteAnimationComponent = characterEntity.GetComponent<SpriteAnimationComponent>();
            var tempPosition = characterEntity.Transform.Position;
            var direction = moveDown ? 1 : -1;

            var sourceRectangle = spriteAnimationComponent.SourceRectangle.Value;
            sourceRectangle.Y = sourcePosY;

            spriteAnimationComponent.IsPlaying = true;
            spriteAnimationComponent.SourceRectangle = sourceRectangle;

            tempPosition.Y += (float)(VELOCITY * Scene.DeltaTime) * direction;

            #region Out of screen in top or bottom
            if (tempPosition.Y + spriteAnimationComponent.FrameHeight < 0f)
                tempPosition.Y = Scene.ScreenHeight;

            if (tempPosition.Y > Scene.ScreenHeight)
                tempPosition.Y = -spriteAnimationComponent.FrameHeight;
            #endregion

            characterEntity.SetPosition(tempPosition);
        }
    }
}
