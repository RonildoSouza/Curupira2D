using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.Common.Scenes;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.Samples.Systems.SpriteAnimation;

namespace MonoGame.Helper.Samples.Scenes
{
    class SpriteAnimationScene : SceneBase
    {
        public override void Initialize()
        {
            SetTitle("SpriteAnimationScene");

            AddSystem<CharacterAnimationSystem>();
            AddSystem<CharacterMovementSystem>();

            // Create entity explosion in scene
            var explosionTexture = GameCore.Content.Load<Texture2D>("SpriteAnimation/explosion");

            CreateEntity("explosion")
                .SetPosition(300, 200)
                .AddComponent(new SpriteAnimationComponent(explosionTexture, 5, 5, 150, AnimateType.All, default, true, true));

            ShowControlTips(120, 40, "MOVIMENT: Keyboard Arrows");

            base.Initialize();
        }
    }
}
