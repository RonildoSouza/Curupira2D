using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Testbed.Common.Systems;
using Microsoft.Xna.Framework.Graphics;

namespace Curupira2D.Testbed.Systems.SceneGraph
{
    [RequiredComponent(typeof(CharacterMovementSystem), typeof(SpriteComponent))]
    class CharacterMovementSystem : EntityMovementSystemBase
    {
        protected override string EntityUniqueId => "character";

        public override void Initialize()
        {
            var characterTexture = Scene.GameCore.Content.Load<Texture2D>("SceneGraph/character");

            Scene.CreateEntity(EntityUniqueId)
                .SetPosition(Scene.ScreenWidth * 0.5f, Scene.ScreenHeight * 0.5f)
                .AddComponent(new SpriteComponent(characterTexture));

            base.Initialize();
        }
    }
}
