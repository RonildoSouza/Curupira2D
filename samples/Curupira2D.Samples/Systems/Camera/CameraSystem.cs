using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Curupira2D.Input;
using Microsoft.Xna.Framework;

namespace Curupira2D.Samples.Systems.Camera
{
    [RequiredComponent(typeof(CameraSystem), typeof(SpriteComponent))]
    class CameraSystem : ECS.System, ILoadable, IUpdatable
    {
        Vector2 _cameraPosition;

        public void LoadContent()
        {
            Scene.GameCore.IsMouseVisible = true;

            var blockTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(100, Color.Red * 0.8f);

            Scene.CreateEntity("block", Scene.ScreenCenter)
                .AddComponent(new SpriteComponent(blockTexture));
        }

        public void Update()
        {

            _cameraPosition.X = Scene.MouseInputManager.GetPosition().X;
            _cameraPosition.Y = Scene.InvertPositionY(Scene.MouseInputManager.GetPosition().Y);

            Scene.Camera2D.Position = _cameraPosition;
            Scene.Camera2D.Zoom = Scene.MouseInputManager.GetScrollWheel() > 0 ? new Vector2(Scene.MouseInputManager.GetScrollWheel() * 0.01f) : Vector2.One;

            if (Scene.MouseInputManager.IsMouseButtonDown(MouseButton.Left))
                Scene.Camera2D.Rotation += 0.01f;

            if (Scene.MouseInputManager.IsMouseButtonPressed(MouseButton.Right))
                Scene.Camera2D.Rotation = 0f;
        }
    }
}
