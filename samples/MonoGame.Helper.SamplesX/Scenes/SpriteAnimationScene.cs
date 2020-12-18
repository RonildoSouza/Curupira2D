using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.Samples.Systems.SpriteAnimation;

namespace MonoGame.Helper.Samples.Scenes
{
    class SpriteAnimationScene : Scene
    {
        public override void Initialize()
        {
            AddSystem<CharacterAnimationSystem>();
            AddSystem<CharacterMovementSystem>();

            // Create entity explosion in scene
            var explosionTexture = GameCore.Content.Load<Texture2D>("SpriteAnimation/explosion");

            CreateEntity("explosion")
                .SetPosition(300, 200)
                .AddComponent(new SpriteAnimationComponent(explosionTexture, 5, 5, 150, AnimateType.All, default, true, true));

            base.Initialize();
        }
    }
}
