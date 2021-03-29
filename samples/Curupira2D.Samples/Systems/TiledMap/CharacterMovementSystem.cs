﻿using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Testbed.Systems.TiledMap
{
    [RequiredComponent(typeof(CharacterMovementSystem), typeof(BodyComponent))]
    class CharacterMovementSystem : ECS.System, IInitializable, IUpdatable
    {
        Entity _characterEntity;
        readonly float _playerVelocity = 200f;
        bool _isMoving;
        bool _isJumping;
        KeyboardState _oldKS;

        public void Initialize()
        {
            var characterTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(40, 80, Color.Black);
            _characterEntity = Scene.CreateEntity("character")
                .AddComponent(new SpriteComponent(characterTexture))
                .AddComponent(new BodyComponent(characterTexture.Bounds.Size.ToVector2(), 0f)
                {
                    EntityType = EntityType.Dynamic,
                    Mass = 1f,
                });
        }

        public void Update()
        {
            var ks = Keyboard.GetState();
            var bodyComponent = _characterEntity.GetComponent<BodyComponent>();

            if (ks.IsKeyDown(Keys.Left))
            {
                bodyComponent.LinearVelocity = new Vector2(-_playerVelocity, _characterEntity.Transform.Position.Y);

                if (Scene.Camera2D.Position.X > Scene.ScreenWidth * 0.5f)
                {
                    var position = Scene.Camera2D.Position;
                    position.X = _characterEntity.Transform.Position.X;
                    Scene.Camera2D.Position = position;
                }

                _isMoving = true;
            }

            if (ks.IsKeyDown(Keys.Right))
            {
                bodyComponent.LinearVelocity = new Vector2(_playerVelocity, _characterEntity.Transform.Position.Y);

                if (Scene.Camera2D.Position.X < _characterEntity.Transform.Position.X)
                {
                    var position = Scene.Camera2D.Position;
                    position.X = _characterEntity.Transform.Position.X;
                    Scene.Camera2D.Position = position;
                }

                _isMoving = true;
            }

            if (ks.IsKeyDown(Keys.Up) && _oldKS.IsKeyUp(Keys.Up))
            {
                //bodyComponent.Force = new Vector2(0f, -1000f);
                _isJumping = true;
            }


            if (!_isMoving && !_isJumping)
                //if (!_isMoving)
                bodyComponent.LinearVelocity = new Vector2(0f, _characterEntity.Transform.Position.Y);

            _isMoving = false;
            _oldKS = ks;
        }
    }
}