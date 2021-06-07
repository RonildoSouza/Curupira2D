/**
 * https://github.com/tainicom/Aether.Physics2D/blob/master/Samples/HelloWorld/Game1.cs
 */

using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.Extensions;
using Curupira2D.Testbed.Common.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Testbed.Scenes
{
    class AetherPhysics2DHelloWorldScene : SceneBase
    {
        Entity _playerEntity;
        readonly float _playerBodyRadius = 1.5f / 2f; // player diameter is 1.5 meters
        readonly Vector2 _groundBodySize = new Vector2(8f, 1f); // ground is 8x1 meters
        Vector2 _cameraPosition = new Vector2(0, 1.70f); // camera is 1.7 meters above the ground

        public override void LoadContent()
        {
            SetTitle(nameof(AetherPhysics2DHelloWorldScene));

            /* Circle */
            var playerPosition = new Vector2(0, _playerBodyRadius);
            var playerTexture = GameCore.Content.Load<Texture2D>("AetherPhysics2D/CircleSprite");

            _playerEntity = CreateEntity("circle")
                .SetPosition(playerPosition)
                .AddComponent(
                    new SpriteComponent(texture: playerTexture, scale: new Vector2(_playerBodyRadius * 2f) / playerTexture.Bounds.Size.ToVector2()),
                    new BodyComponent(_playerBodyRadius, EntityType.Dynamic)
                    {
                        Restitution = 0.3f,
                        Friction = 0.5f,
                    });

            /* Ground */
            var groundPosition = new Vector2(0, _groundBodySize.Y * -0.5f);
            var groundTexture = GameCore.Content.Load<Texture2D>("AetherPhysics2D/GroundSprite");

            CreateEntity("ground")
                .SetPosition(groundPosition)
                .AddComponent(
                    new SpriteComponent(texture: groundTexture, scale: _groundBodySize / groundTexture.Bounds.Size.ToVector2()),
                    new BodyComponent(_groundBodySize, EntityType.Static, EntityShape.Rectangle)
                    {
                        Restitution = 0.3f,
                        Friction = 0.5f,
                    });

            ShowControlTips(140, 60, "Press A or D to rotate the ball\n" +
                                     "Press Space to jump\n" +
                                     "Use arrow keys to move the camera");

            base.LoadContent();

            Camera2D.Position = _cameraPosition;
            Camera2D.Zoom = new Vector2(0.03f);
        }

        private KeyboardState _oldKeyState;

        public override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var bodyComponent = _playerEntity.GetComponent<BodyComponent>();

            // Move camera
            if (state.IsKeyDown(Keys.Left))
                _cameraPosition.X += 12f * DeltaTime;

            if (state.IsKeyDown(Keys.Right))
                _cameraPosition.X -= 12f * DeltaTime;

            if (state.IsKeyDown(Keys.Up))
                _cameraPosition.Y -= 12f * DeltaTime;

            if (state.IsKeyDown(Keys.Down))
                _cameraPosition.Y += 12f * DeltaTime;

            // We make it possible to rotate the player body
            if (state.IsKeyDown(Keys.A))
                bodyComponent.ApplyTorque(10);

            if (state.IsKeyDown(Keys.D))
                bodyComponent.ApplyTorque(-10);

            if (state.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
                bodyComponent.ApplyLinearImpulse(new Vector2(0f, 10f));

            _oldKeyState = state;
            Camera2D.Position = _cameraPosition;

            base.Update(gameTime);
        }
    }
}
