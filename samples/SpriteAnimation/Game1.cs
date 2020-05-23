using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Components.Physics;

namespace SpriteAnimation
{
    public class Game1 : GameCore
    {
        public Game1() : base() { }

        protected override void Initialize()
        {
            var scene = new Scene()
                .AddSystem<CharacterMovimentSystem>();

            var characterTexture = Content.Load<Texture2D>("character");

            scene.CreateEntity("character")
                .SetPosition(100, 100)
                .AddComponent(new TransformComponent { Velocity = new Vector2(100) })
                .AddComponent(new SpriteAnimationComponent(characterTexture, 4, 4, 100, AnimateType.PerRow));
            //.AddComponent(new SpriteComponent(texture));

            var explosionTexture = Content.Load<Texture2D>("explosion");

            scene.CreateEntity("explosion")
                .SetPosition(300, 200)
                .AddComponent(new SpriteAnimationComponent(explosionTexture, 5, 5, 150, AnimateType.All, default, true, true));

            SetScene(scene);
        }
    }
}
