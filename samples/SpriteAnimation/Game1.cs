using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;

namespace SpriteAnimation
{
    public class Game1 : GameCore
    {
        public Game1() : base() { }

        protected override void Initialize()
        {
            var scene = new Scene()
                .AddSystem<CharacterMovimentSystem>();

            // Create entity character in scene
            var characterTexture = Content.Load<Texture2D>("character");

            scene.CreateEntity("character")
                .SetPosition(100, 100)
                //.AddComponent(new SpriteComponent(texture));
                .AddComponent(new SpriteAnimationComponent(characterTexture, 4, 4, 100, AnimateType.PerRow));

            // Create entity explosion in scene
            var explosionTexture = Content.Load<Texture2D>("explosion");

            scene.CreateEntity("explosion")
                .SetPosition(300, 200)
                .AddComponent(new SpriteAnimationComponent(explosionTexture, 5, 5, 150, AnimateType.All, default, true, true));

            // Set initial game scene
            SetScene(scene);
        }
    }
}
