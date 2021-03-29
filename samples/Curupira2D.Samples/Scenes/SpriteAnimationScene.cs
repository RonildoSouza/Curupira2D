using Curupira2D.ECS.Components.Drawables;
using Curupira2D.Testbed.Common.Scenes;
using Curupira2D.Testbed.Systems.SpriteAnimation;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.Testbed.Scenes
{
    class SpriteAnimationScene : SceneBase
    {
        public override void Initialize()
        {
            SetTitle(nameof(SpriteAnimationScene));

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
