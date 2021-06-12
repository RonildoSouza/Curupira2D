using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Samples.Systems.TiledMap
{
    [RequiredComponent(typeof(CharacterMovementSystem), typeof(BodyComponent))]
    class CharacterMovementSystem : ECS.System, ILoadable, IUpdatable
    {
        Entity _characterEntity;
        readonly float _velocity = 200f;
        bool _isMoving;
        bool _isJumping;

        public void LoadContent()
        {
            var characterTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(40, 80, Color.Black);

            _characterEntity = Scene.CreateEntity("character")
                .AddComponent(
                    new SpriteComponent(characterTexture),
                    new BodyComponent(characterTexture.Bounds.Size.ToVector2(), EntityType.Dynamic, EntityShape.Rectangle, 0f)
                    {
                        FixedRotation = true
                    });
        }

        public void Update()
        {
            var bodyComponent = _characterEntity.GetComponent<BodyComponent>();

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Right))
            {
                bodyComponent.ApplyForce(new Vector2(_velocity, 0f));

                if (Scene.Camera2D.Position.X < _characterEntity.Transform.Position.X)
                {
                    var position = Scene.Camera2D.Position;
                    position.X = _characterEntity.Transform.Position.X;
                    Scene.Camera2D.Position = position;
                }

                _isMoving = true;
            }

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Left))
            {
                bodyComponent.ApplyForce(new Vector2(-_velocity, 0f));

                if (Scene.Camera2D.Position.X > Scene.ScreenCenter.X)
                {
                    var position = Scene.Camera2D.Position;
                    position.X = _characterEntity.Transform.Position.X;
                    Scene.Camera2D.Position = position;
                }

                _isMoving = true;
            }

            if (Scene.KeyboardInputManager.IsKeyPressed(Keys.Up))
            {
                //_isJumping = true;

                bodyComponent.ApplyLinearImpulse(new Vector2(bodyComponent.LinearVelocity.X, 20f));
            }

            if (!_isMoving)
            {
                Scene.Camera2D.Position = Scene.Camera2D.Position;
                bodyComponent.SetLinearVelocityX(0f);
            }

            _isMoving = false;
        }
    }
}
