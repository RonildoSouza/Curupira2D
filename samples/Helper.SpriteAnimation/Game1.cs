using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using Helper.SpriteAnimation.Systems;

namespace Helper.SpriteAnimation
{
    public class Game1 : GameCore
    {
        public Game1() : base() { }

        protected override void Initialize()
        {
            var scene = new Scene()
                .AddSystem<CharacterAnimationSystem>()
                .AddSystem<CharacterMovementSystem>()
                .AddSystem<TextSystem>();

            // Create entity explosion in scene
            var explosionTexture = Content.Load<Texture2D>("explosion");

            scene.CreateEntity("explosion")
                .SetPosition(300, 200)
                .AddComponent(new SpriteAnimationComponent(explosionTexture, 5, 5, 150, AnimateType.All, default, true, true));

            #region Controls Tips
            var fontArial = Content.Load<SpriteFont>("FontArial");
            scene.CreateEntity("controls")
                .SetPosition(120, 40)
                .AddComponent(new TextComponent(
                    fontArial,
                    "CONTROLS" +
                    "\nMOVIMENT: Keyboard Arrows",
                    color: Color.Black));
            #endregion

            // Set initial game scene
            SetScene(scene);
        }
    }
}
