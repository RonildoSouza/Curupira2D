using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Testbed.Systems.SpriteAnimation
{
    [RequiredComponent(typeof(CharacterAnimationSystem), typeof(SpriteAnimationComponent))]
    class CharacterAnimationSystem : ECS.System, ILoadable, IUpdatable
    {
        Entity _characterEntity;

        public void LoadContent()
        {
            // Create entity character in scene
            var characterTexture = Scene.GameCore.Content.Load<Texture2D>("SpriteAnimation/character");

            _characterEntity = Scene.CreateEntity("character")
                .SetPosition(100, 100)
                .AddComponent(new SpriteAnimationComponent(characterTexture, 4, 4, 100, AnimateType.PerRow));
        }

        public void Update()
        {
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Left))
                HorizontalAnimation(ref _characterEntity, 180);

            if (keyState.IsKeyDown(Keys.Up))
                VerticalAnimation(ref _characterEntity, 90);

            if (keyState.IsKeyDown(Keys.Right))
                HorizontalAnimation(ref _characterEntity, 270);

            if (keyState.IsKeyDown(Keys.Down))
                VerticalAnimation(ref _characterEntity, 0);
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
