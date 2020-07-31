using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.Extensions;

namespace Helper.Camera
{
    public class Game1 : GameCore
    {
        Scene _scene;
        public Game1() : base(debugActive: true)
        {
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _scene = new Scene();

            var blockTexture = GraphicsDevice.CreateTextureRectangle(100, Color.Red * 0.8f);

            _scene.CreateEntity("block")
                .SetPosition(GraphicsDevice.Viewport.Width * 0.5f, GraphicsDevice.Viewport.Height * 0.5f)
                .AddComponent(new SpriteComponent(blockTexture));

            SetScene(_scene);

            _scene.Camera.Position = new Vector2(GraphicsDevice.Viewport.Width * 0.5f, GraphicsDevice.Viewport.Height * 0.5f);
            _scene.Camera.Debug.IsVisible = true;
            _scene.Camera.Debug.Grid.AddLines(50, Color.White, 2);

            #region Controls Tips
            var fontArial = Content.Load<SpriteFont>("FontArial");
            _scene.CreateEntity("controls")
                .SetPosition(140, 200)
                .AddComponent(new TextComponent(
                    fontArial,
                    "CONTROLS" +
                    "\nMOVIMENT: Mouse Cursor" +
                    "\nZOOM: Mouse Wheel" +
                    "\nROTATION: Mouse Left Button" +
                    "\nROTATION: Mouse Right Button",
                    color: Color.Black,
                    fixedPosition: true));
            #endregion

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            var ms = Mouse.GetState();

            _scene.Camera.Position = ms.Position.ToVector2();
            _scene.Camera.Zoom = ms.ScrollWheelValue > 0 ? ms.ScrollWheelValue * 0.01f : 1;

            if (ms.LeftButton == ButtonState.Pressed)
                _scene.Camera.Rotation += 0.01f;

            if (ms.RightButton == ButtonState.Pressed)
                _scene.Camera.Rotation = 0f;

            base.Update(gameTime);
        }
    }
}
