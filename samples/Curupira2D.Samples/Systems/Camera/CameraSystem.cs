using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Testbed.Systems.Camera
{
    [RequiredComponent(typeof(CameraSystem), typeof(SpriteComponent))]
    class CameraSystem : ECS.System, ILoadable, IUpdatable
    {
        Vector2 _cameraPosition;

        public void LoadContent()
        {
            Scene.GameCore.IsMouseVisible = true;

            var blockTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(100, Color.Red * 0.8f);

            Scene.CreateEntity("block")
                .SetPosition(Scene.ScreenWidth * 0.5f, Scene.ScreenHeight * 0.5f)
                .AddComponent(new SpriteComponent(texture: blockTexture, drawWithoutUsingCamera: false));
        }

        public void Update()
        {
            var ms = Mouse.GetState();

            _cameraPosition.X = ms.Position.X;
            _cameraPosition.Y = Scene.InvertPositionY(ms.Position.Y);

            Scene.Camera2D.Position = _cameraPosition;
            Scene.Camera2D.Zoom = ms.ScrollWheelValue > 0 ? new Vector2(ms.ScrollWheelValue * 0.01f) : Vector2.One;

            if (ms.LeftButton == ButtonState.Pressed)
                Scene.Camera2D.Rotation += 0.01f;

            if (ms.RightButton == ButtonState.Pressed)
                Scene.Camera2D.Rotation = 0f;
        }
    }
}
