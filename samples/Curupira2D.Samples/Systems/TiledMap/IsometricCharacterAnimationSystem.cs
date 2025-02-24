using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Samples.Systems.TiledMap
{
    [RequiredComponent(typeof(IsometricCharacterAnimationSystem), typeof(SpriteAnimationComponent))]
    class IsometricCharacterAnimationSystem : ECS.System, ILoadable, IUpdatable
    {
        SpriteAnimationComponent _spriteAnimationComponent;

        public void LoadContent()
        {
            // Create entity character in scene
            var characterTexture = Scene.GameCore.Content.Load<Texture2D>("TiledMap/IsometricBicycle");

            _spriteAnimationComponent = new SpriteAnimationComponent(characterTexture, 8, 8, 100, AnimateType.PerRow, layerDepth: 0.01f, drawInUICamera: true);
            Scene.CreateEntity("isometricCharacter", Scene.ScreenWidth * 0.3f, Scene.ScreenCenter.Y)
                .AddComponent(
                    _spriteAnimationComponent/*,
                    new BodyComponent(characterTexture.Bounds.Size.ToVector2() / 8, EntityType.Dynamic, EntityShape.Rectangle)
                    {
                        IgnoreGravity = true,
                        LinearDamping = 1f,
                        AngularDamping = 1f,
                        Friction = 0.5f,
                    }*/);
        }

        public void Update()
        {
            _spriteAnimationComponent.IsPlaying = false;

            // Horizontal and Vertical direction
            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Left) || Scene.KeyboardInputManager.IsKeyDown(Keys.A))
                HorizontalAnimation(0);

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Up) || Scene.KeyboardInputManager.IsKeyDown(Keys.W))
                VerticalAnimation(128);

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Right) || Scene.KeyboardInputManager.IsKeyDown(Keys.D))
                HorizontalAnimation(256);

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Down) || Scene.KeyboardInputManager.IsKeyDown(Keys.S))
                VerticalAnimation(384);

            // Diagonal direction
            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Left) && Scene.KeyboardInputManager.IsKeyDown(Keys.Up)
                || Scene.KeyboardInputManager.IsKeyDown(Keys.A) && Scene.KeyboardInputManager.IsKeyDown(Keys.W))
                DiagonalAnimation(64);

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Right) && Scene.KeyboardInputManager.IsKeyDown(Keys.Up)
                || Scene.KeyboardInputManager.IsKeyDown(Keys.D) && Scene.KeyboardInputManager.IsKeyDown(Keys.W))
                DiagonalAnimation(192);

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Right) && Scene.KeyboardInputManager.IsKeyDown(Keys.Down)
                || Scene.KeyboardInputManager.IsKeyDown(Keys.D) && Scene.KeyboardInputManager.IsKeyDown(Keys.S))
                DiagonalAnimation(320);

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Left) && Scene.KeyboardInputManager.IsKeyDown(Keys.Down)
                || Scene.KeyboardInputManager.IsKeyDown(Keys.A) && Scene.KeyboardInputManager.IsKeyDown(Keys.S))
                DiagonalAnimation(448);
        }

        void HorizontalAnimation(int sourcePosY)
        {
            var sourceRectangle = _spriteAnimationComponent.SourceRectangle.Value;
            sourceRectangle.Y = sourcePosY;

            _spriteAnimationComponent.IsPlaying = true;
            _spriteAnimationComponent.SourceRectangle = sourceRectangle;
        }

        void VerticalAnimation(int sourcePosY)
        {
            var sourceRectangle = _spriteAnimationComponent.SourceRectangle.Value;
            sourceRectangle.Y = sourcePosY;

            _spriteAnimationComponent.IsPlaying = true;
            _spriteAnimationComponent.SourceRectangle = sourceRectangle;
        }

        void DiagonalAnimation(int sourcePosY)
        {
            var sourceRectangle = _spriteAnimationComponent.SourceRectangle.Value;
            sourceRectangle.Y = sourcePosY;

            _spriteAnimationComponent.IsPlaying = true;
            _spriteAnimationComponent.SourceRectangle = sourceRectangle;
        }
    }
}
