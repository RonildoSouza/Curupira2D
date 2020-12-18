using Collision.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;

namespace Helper.Physic.Collision
{
    public class Game1 : GameCore
    {
        public Game1() : base(debugActive: true) { }

        protected override void Initialize()
        {
            var scene = new Scene()
                .AddSystem<BallControllerSystem>()
                .AddSystem<SquareControllerSystem>()
                .AddSystem<BorderControllerSystem>();

            #region Controls Tips
            var fontArial = Content.Load<SpriteFont>("FontArial");
            scene.CreateEntity("controls")
                .SetPosition(140, 60)
                .AddComponent(new TextComponent(
                    fontArial,
                    "CONTROLS" +
                    "\nMOVIMENT: Keyboard Arrows",
                    color: Color.Black));
            #endregion

            SetScene(scene);

            base.Initialize();
        }
    }
}
