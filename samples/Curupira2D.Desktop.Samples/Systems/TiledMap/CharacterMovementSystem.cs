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
    [RequiredComponent(typeof(CharacterMovementSystem), [typeof(SpriteComponent), typeof(BodyComponent)])]
    class CharacterMovementSystem : ECS.System, ILoadable, IUpdatable
    {
        Entity _characterEntity;
        BodyComponent _characterBodyComponent;
        float _impulse;

        public void LoadContent()
        {
            Scene.Gravity = new Vector2(0f, Scene.Gravity.Y * 6f);

            var characterTexture = Scene.GameCore.Content.Load<Texture2D>("TiledMap/Ball");
            var characterRadius = characterTexture.Bounds.Size.X * 0.1f;
            _characterBodyComponent = new BodyComponent(characterRadius, EntityType.Dynamic)
            {
                Restitution = 0.3f,
                Friction = 0.5f,
            };

            _characterEntity = Scene.CreateEntity("character", default)
                .AddComponent(new SpriteComponent(characterTexture, scale: new Vector2(0.2f), layerDepth: 1f), _characterBodyComponent);
        }

        public void Update()
        {
            var cameraPosition = Vector2.Lerp(_characterEntity.Position, Scene.Camera2D.Position, 0.125f);

            if (_impulse == 0f)
                _impulse = _characterBodyComponent.Mass * 10 / Scene.DeltaTime;

            // Moving updates
            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Right) || Scene.KeyboardInputManager.IsKeyDown(Keys.D))
            {
                _characterBodyComponent.ApplyTorque(-_impulse * 5);

                if (Scene.Camera2D.Position.X < _characterEntity.Position.X)
                    Scene.Camera2D.Position = new Vector2(cameraPosition.X, Scene.Camera2D.Position.Y);
            }

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Left) || Scene.KeyboardInputManager.IsKeyDown(Keys.A))
            {
                _characterBodyComponent.ApplyTorque(_impulse * 5);

                if (Scene.Camera2D.Position.X > Scene.ScreenCenter.X)
                    Scene.Camera2D.Position = new Vector2(cameraPosition.X, Scene.Camera2D.Position.Y);
            }

            // Jumping updates
            if (Scene.KeyboardInputManager.IsKeyPressed(Keys.Up) || Scene.KeyboardInputManager.IsKeyDown(Keys.W) || Scene.KeyboardInputManager.IsKeyPressed(Keys.Space))
                _characterBodyComponent.ApplyLinearImpulseY(_impulse);

            if (_characterEntity.Position.Y < Scene.ScreenCenter.Y)
                Scene.Camera2D.Position = new Vector2(cameraPosition.X, cameraPosition.Y + Scene.ScreenCenter.Y * 0.45f);
        }
    }
}
