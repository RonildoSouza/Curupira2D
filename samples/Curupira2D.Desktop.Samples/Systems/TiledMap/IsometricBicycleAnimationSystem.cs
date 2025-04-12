using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Desktop.Samples.Systems.TiledMap
{
    [RequiredComponent(typeof(IsometricBicycleAnimationSystem), [typeof(SpriteAnimationComponent), typeof(BodyComponent)])]
    class IsometricBicycleAnimationSystem : ECS.System, ILoadable, IUpdatable
    {
        Entity _isometricCharacterEntity;
        SpriteAnimationComponent _spriteAnimationComponent;
        BodyComponent _isometricCharacterBodyComponent;

        public void LoadContent()
        {
            // Create entity character in scene
            var characterTexture = Scene.GameCore.Content.Load<Texture2D>("TiledMap/IsometricBicycle");

            _spriteAnimationComponent = new SpriteAnimationComponent(characterTexture, 8, 8, 100, AnimateType.PerRow, layerDepth: 0.01f);
            _isometricCharacterBodyComponent = new BodyComponent((characterTexture.Bounds.Size.ToVector2() / 8f).X / 2.2f, EntityType.Dynamic)
            {
                IgnoreGravity = true,
                FixedRotation = true,
                LinearDamping = 1f,
                Friction = 0.5f,
            };

            _isometricCharacterEntity = Scene.CreateEntity("isometricBicycle", default)
                .AddComponent(_spriteAnimationComponent, _isometricCharacterBodyComponent);
        }

        public void Update()
        {
            _spriteAnimationComponent.IsPlaying = false;
            var direction = Vector2.Zero;

            direction = HandleIsometricMovement(direction);
            ApplyIsometricVelocity(direction);

            Scene.Camera2D.Position = Vector2.Lerp(_isometricCharacterEntity.Position, Scene.Camera2D.Position, 0.125f);
        }

        Vector2 HandleIsometricMovement(Vector2 direction)
        {
            #region Isometric Horizontal and Vertical Movement
            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Left))
            {
                ApplyBicycleIsometricAnimation(IsometricBicycleDirection.Left);
                direction = new Vector2(0, 1);
            }

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Up))
            {
                ApplyBicycleIsometricAnimation(IsometricBicycleDirection.Up);
                direction = new Vector2(1, 0);
            }

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Right))
            {
                ApplyBicycleIsometricAnimation(IsometricBicycleDirection.Right);
                direction = new Vector2(0, -1);
            }

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Down))
            {
                ApplyBicycleIsometricAnimation(IsometricBicycleDirection.Down);
                direction = new Vector2(-1, 0);
            }
            #endregion

            #region Isometric Diagonal Movement
            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Left) && Scene.KeyboardInputManager.IsKeyDown(Keys.Up))
            {
                ApplyBicycleIsometricAnimation(IsometricBicycleDirection.LeftUp);
                direction = new Vector2(1, 1);
            }

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Right) && Scene.KeyboardInputManager.IsKeyDown(Keys.Up))
            {
                ApplyBicycleIsometricAnimation(IsometricBicycleDirection.RightUp);
                direction = new Vector2(1, -1);
            }

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Left) && Scene.KeyboardInputManager.IsKeyDown(Keys.Down))
            {
                ApplyBicycleIsometricAnimation(IsometricBicycleDirection.LeftDown);
                direction = new Vector2(-1, 1);
            }

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Right) && Scene.KeyboardInputManager.IsKeyDown(Keys.Down))
            {
                ApplyBicycleIsometricAnimation(IsometricBicycleDirection.RightDown);
                direction = new Vector2(-1, -1);
            }
            #endregion

            return direction;
        }

        void ApplyBicycleIsometricAnimation(IsometricBicycleDirection isometricBicycleDirection)
        {
            var sourceRectangle = _spriteAnimationComponent.SourceRectangle.Value;
            sourceRectangle.Y = (int)isometricBicycleDirection;

            _spriteAnimationComponent.IsPlaying = true;
            _spriteAnimationComponent.SourceRectangle = sourceRectangle;
        }

        void ApplyIsometricVelocity(Vector2 direction)
        {
            if (direction == Vector2.Zero)
                _isometricCharacterBodyComponent.LinearVelocity = Vector2.Zero;

            var linearImpulse = _isometricCharacterBodyComponent.Mass * 4f;
            var impulse = new Vector2(direction.X * linearImpulse, direction.Y * linearImpulse);

            _isometricCharacterBodyComponent.ApplyLinearImpulse(impulse.CartesianToIsometric());
            _isometricCharacterBodyComponent.Position.Normalize();
        }
    }
}
