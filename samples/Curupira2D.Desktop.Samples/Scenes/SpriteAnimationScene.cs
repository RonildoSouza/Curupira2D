using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.Desktop.Samples.Systems.SpriteAnimation;
using Curupira2D.ECS.Components.Drawables;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.Desktop.Samples.Scenes
{
    class SpriteAnimationScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(SpriteAnimationScene));

            AddSystem<CharacterAnimationSystem>();
            AddSystem<CharacterMovementSystem>();

            // Create entity explosion in scene
            var explosionTexture = GameCore.Content.Load<Texture2D>("SpriteAnimation/explosion");

            CreateEntity("explosion", ScreenCenter)
                .AddComponent(new SpriteAnimationComponent(explosionTexture, 5, 5, 150, AnimateType.All, default, true, true));

            ShowControlTips("MOVIMENT: Keyboard Arrows OR WASD");

            base.LoadContent();
        }
    }
}
