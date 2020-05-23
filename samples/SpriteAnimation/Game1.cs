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

            var texture = Content.Load<Texture2D>("character");

            scene.CreateEntity("character")
                .SetPosition(100, 100)
                .AddComponent(new TransformComponent { Velocity = new Vector2(100) })
                .AddComponent(new SpriteAnimationComponent(texture, 4, 4, 100, AnimateType.PerRow));
            //.AddComponent(new SpriteComponent(texture));

            SetScene(scene);
        }
    }
}
