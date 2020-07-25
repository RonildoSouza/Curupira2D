using Collision.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using MonoGame.Helper.Physic;

namespace Helper.Physic.Collision
{
    public class Game1 : GameCore
    {
        public Game1() : base() { }

        protected override void Initialize()
        {
            var scenePhysics = new ScenePhysics(default, true)
                .AddSystem<BallControllerSystem>()
                .AddSystem<SquareControllerSystem>()
                .AddSystem<BorderControllerSystem>()
                .AddSystem<TextSystem>();

            #region Controls Tips
            var fontArial = Content.Load<SpriteFont>("FontArial");
            scenePhysics.CreateEntity("controls")
                .SetPosition(140, 60)
                .AddComponent(new TextComponent(
                    fontArial,
                    "CONTROLS" +
                    "\nMOVIMENT: Keyboard Arrows",
                    color: Color.Black));
            #endregion

            SetScene(scenePhysics);
        }
    }
}
