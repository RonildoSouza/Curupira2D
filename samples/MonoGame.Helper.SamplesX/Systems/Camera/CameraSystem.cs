using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using MonoGame.Helper.Extensions;

namespace MonoGame.Helper.Samples.Systems.Camera
{
    [RequiredComponent(typeof(SpriteComponent))]
    class CameraSystem : ECS.System, IInitializable, IUpdatable
    {
        public void Initialize()
        {
            Scene.GameCore.IsMouseVisible = true;

            var blockTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(100, Color.Red * 0.8f);

            Scene.CreateEntity("block")
                .SetPosition(Scene.ScreenWidth * 0.5f, Scene.ScreenHeight * 0.5f)
                .AddComponent(new SpriteComponent(blockTexture));

            Scene.Camera.Position = new Vector2(Scene.ScreenWidth * 0.5f, Scene.ScreenHeight * 0.5f);
            Scene.Camera.Debug.IsVisible = true;
            Scene.Camera.Debug.Grid.AddLines(50, Color.White, 2);
        }

        public void Update()
        {
            var ms = Mouse.GetState();

            Scene.Camera.Position = ms.Position.ToVector2();
            Scene.Camera.Zoom = ms.ScrollWheelValue > 0 ? ms.ScrollWheelValue * 0.01f : 1;

            if (ms.LeftButton == ButtonState.Pressed)
                Scene.Camera.Rotation += 0.01f;

            if (ms.RightButton == ButtonState.Pressed)
                Scene.Camera.Rotation = 0f;
        }
    }
}
