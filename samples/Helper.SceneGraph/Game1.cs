using Helper.SceneGraph.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;

namespace Helper.SceneGraph
{
    public class Game1 : GameCore
    {
        public Game1() : base(debugActive: true) { }

        protected override void Initialize()
        {
            var scene = new Scene()
                .AddSystem<CharacterMovementSystem>()
                .AddSystem<EquipmentMovimentSystem>();

            #region Controls Tips
            var fontArial = Content.Load<SpriteFont>("FontArial");
            scene.CreateEntity("controls")
                .SetPosition(120, 50)
                .AddComponent(new TextComponent(
                    fontArial,
                    "CONTROLS" +
                    "\nMOVIMENT: Keyboard Arrows" +
                    "\nEQUIPMENTS: Key 1, 2",
                    color: Color.Black));
            #endregion

            SetScene(scene);
        }
    }
}
