using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;

namespace Helper.SpriteAnimation.Systems
{
    [RequiredComponent(typeof(SpriteAnimationComponent))]
    public class CharacterMovementSystem : MonoGame.Helper.ECS.System, IUpdatable
    {
        const float VELOCITY = 100f;

        public void Update()
        {
            var characterEntity = Scene.GetEntity("character");
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Left))
                HorizontalMove(ref characterEntity, false);

            if (ks.IsKeyDown(Keys.Up))
                VerticalMove(ref characterEntity, false);

            if (ks.IsKeyDown(Keys.Right))
                HorizontalMove(ref characterEntity);

            if (ks.IsKeyDown(Keys.Down))
                VerticalMove(ref characterEntity);
        }

        void HorizontalMove(ref Entity characterEntity, bool moveRight = true)
        {
            var spriteAnimationComponent = characterEntity.GetComponent<SpriteAnimationComponent>();
            var tempPosition = characterEntity.Transform.Position;
            var direction = moveRight ? 1 : -1;

            tempPosition.X += (float)(VELOCITY * Scene.DeltaTime) * direction;

            #region Out of screen in left or right
            if (tempPosition.X + spriteAnimationComponent.FrameWidth < 0f)
                tempPosition.X = Scene.ScreenWidth;

            if (tempPosition.X > Scene.ScreenWidth)
                tempPosition.X = -spriteAnimationComponent.FrameWidth;
            #endregion

            characterEntity.SetPosition(tempPosition);
        }

        void VerticalMove(ref Entity characterEntity, bool moveDown = true)
        {
            var spriteAnimationComponent = characterEntity.GetComponent<SpriteAnimationComponent>();
            var tempPosition = characterEntity.Transform.Position;
            var direction = moveDown ? 1 : -1;

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
