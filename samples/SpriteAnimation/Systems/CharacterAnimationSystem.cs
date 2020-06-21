using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;

namespace SpriteAnimation.Systems
{
    [RequiredComponent(typeof(SpriteAnimationComponent))]
    public class CharacterAnimationSystem : MonoGame.Helper.ECS.System, IUpdatable
    {
        public void Update()
        {
            var characterEntity = Scene.GetEntity("character");

            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Left))
                HorizontalAnimation(ref characterEntity, 180);

            if (ks.IsKeyDown(Keys.Up))
                VerticalAnimation(ref characterEntity, 90);

            if (ks.IsKeyDown(Keys.Right))
                HorizontalAnimation(ref characterEntity, 270);

            if (ks.IsKeyDown(Keys.Down))
                VerticalAnimation(ref characterEntity, 0);
        }

        void HorizontalAnimation(ref Entity characterEntity, int sourcePosY)
        {
            var spriteAnimationComponent = characterEntity.GetComponent<SpriteAnimationComponent>();

            var sourceRectangle = spriteAnimationComponent.SourceRectangle.Value;
            sourceRectangle.Y = sourcePosY;

            spriteAnimationComponent.IsPlaying = true;
            spriteAnimationComponent.SourceRectangle = sourceRectangle;
        }

        void VerticalAnimation(ref Entity characterEntity, int sourcePosY)
        {
            var spriteAnimationComponent = characterEntity.GetComponent<SpriteAnimationComponent>();

            var sourceRectangle = spriteAnimationComponent.SourceRectangle.Value;
            sourceRectangle.Y = sourcePosY;

            spriteAnimationComponent.IsPlaying = true;
            spriteAnimationComponent.SourceRectangle = sourceRectangle;
        }
    }
}
