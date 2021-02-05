using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using MonoGame.Helper.ECS.Systems.Attributes;

namespace MonoGame.Helper.Samples.Systems.SpriteAnimation
{
    [RequiredComponent(typeof(CharacterAnimationSystem), typeof(SpriteAnimationComponent))]
    class CharacterAnimationSystem : ECS.System, IInitializable, IUpdatable
    {
        Entity _characterEntity;

        public void Initialize()
        {
            // Create entity character in scene
            var characterTexture = Scene.GameCore.Content.Load<Texture2D>("SpriteAnimation/character");

            _characterEntity = Scene.CreateEntity("character")
                .SetPosition(100, 100)
                .AddComponent(new SpriteAnimationComponent(characterTexture, 4, 4, 100, AnimateType.PerRow));
        }

        public void Update()
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Left))
                HorizontalAnimation(ref _characterEntity, 180);

            if (ks.IsKeyDown(Keys.Up))
                VerticalAnimation(ref _characterEntity, 90);

            if (ks.IsKeyDown(Keys.Right))
                HorizontalAnimation(ref _characterEntity, 270);

            if (ks.IsKeyDown(Keys.Down))
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
