using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Desktop.Samples.Systems.SpriteAnimation
{
    [RequiredComponent(typeof(CharacterAnimationSystem), typeof(SpriteAnimationComponent))]
    class CharacterAnimationSystem : ECS.System, ILoadable, IUpdatable
    {
        SpriteAnimationComponent _spriteAnimationComponent;

        public void LoadContent()
        {
            // Create entity character in scene
            var characterTexture = Scene.GameCore.Content.Load<Texture2D>("SpriteAnimation/character");

            _spriteAnimationComponent = new SpriteAnimationComponent(characterTexture, 4, 4, 100, AnimateType.PerRow);
            Scene.CreateEntity("character", Scene.ScreenWidth * 0.3f, Scene.ScreenCenter.Y)
                .AddComponent(_spriteAnimationComponent);
        }

        public void Update()
        {
            _spriteAnimationComponent.IsPlaying = false;

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Left) || Scene.KeyboardInputManager.IsKeyDown(Keys.A))
                ApplyWalkAnimation(WalkDirection.Left);

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Up) || Scene.KeyboardInputManager.IsKeyDown(Keys.W))
                ApplyWalkAnimation(WalkDirection.Up);

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Right) || Scene.KeyboardInputManager.IsKeyDown(Keys.D))
                ApplyWalkAnimation(WalkDirection.Right);

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Down) || Scene.KeyboardInputManager.IsKeyDown(Keys.S))
                ApplyWalkAnimation(WalkDirection.Down);
        }

        void ApplyWalkAnimation(WalkDirection walkDirection)
        {
            var sourceRectangle = _spriteAnimationComponent.SourceRectangle.Value;
            sourceRectangle.Y = (int)walkDirection;

            _spriteAnimationComponent.IsPlaying = true;
            _spriteAnimationComponent.SourceRectangle = sourceRectangle;
        }
    }
}
