using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Curupira2D.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Samples.Systems.Camera
{
    [RequiredComponent(typeof(CameraSystem), typeof(SpriteComponent))]
    class CameraSystem : ECS.System, ILoadable, IUpdatable
    {
        Vector2 _cameraPosition;
        readonly bool _moveWithKeyboard;

        public CameraSystem(bool moveWithKeyboard)
        {
            _moveWithKeyboard = moveWithKeyboard;
        }

        public void LoadContent()
        {
            Scene.GameCore.IsMouseVisible = true;
            Scene.Camera2D.Position = Scene.ScreenCenter;
        }

        public void Update()
        {
            if (_moveWithKeyboard)
            {
                _cameraPosition = Scene.Camera2D.Position;
                var direction = Vector2.Zero;

                if (Scene.KeyboardInputManager.IsKeyDown(Keys.A))
                    direction.X -= 1;

                if (Scene.KeyboardInputManager.IsKeyDown(Keys.W))
                    direction.Y += 1;

                if (Scene.KeyboardInputManager.IsKeyDown(Keys.D))
                    direction.X += 1;

                if (Scene.KeyboardInputManager.IsKeyDown(Keys.S))
                    direction.Y -= 1;

                _cameraPosition += (float)(500f * Scene.DeltaTime) * direction.GetSafeNormalize();
            }
            else
            {
                _cameraPosition.X = Scene.MouseInputManager.GetPosition().X;
                _cameraPosition.Y = Scene.InvertPositionY(Scene.MouseInputManager.GetPosition().Y);
            }

            Scene.Camera2D.Position = _cameraPosition;
            Scene.Camera2D.Zoom = Scene.MouseInputManager.GetScrollWheel() < 0 ? new Vector2(Scene.MouseInputManager.GetScrollWheel() * -0.01f) : Vector2.One;

            if (Scene.MouseInputManager.IsMouseButtonDown(MouseButton.Left))
                Scene.Camera2D.Rotation += 0.01f;

            if (Scene.MouseInputManager.IsMouseButtonPressed(MouseButton.Right))
                Scene.Camera2D.Rotation = 0f;
        }
    }
}
