using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using MonoGame.Helper.ECS.Systems.Attributes;
using MonoGame.Helper.Extensions;

namespace MonoGame.Helper.Samples.Systems.Camera
{
    [RequiredComponent(typeof(CameraSystem), typeof(SpriteComponent))]
    class CameraSystem : ECS.System, IInitializable, IUpdatable
    {
        public void Initialize()
        {
            Scene.GameCore.IsMouseVisible = true;

            var blockTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(100, Color.Red * 0.8f);

            Scene.CreateEntity("block")
                .SetPosition(Scene.ScreenWidth * 0.5f, Scene.ScreenHeight * 0.5f)
                .AddComponent(new SpriteComponent(blockTexture));
        }

        public void Update()
        {
            var ms = Mouse.GetState();

            Scene.Camera2D.Position = ms.Position.ToVector2();
            Scene.Camera2D.Zoom = ms.ScrollWheelValue > 0 ? new Vector2(ms.ScrollWheelValue * 0.01f) : Vector2.One;

            if (ms.LeftButton == ButtonState.Pressed)
                Scene.Camera2D.Rotation += 0.01f;

            if (ms.RightButton == ButtonState.Pressed)
                Scene.Camera2D.Rotation = 0f;
        }
    }
}
