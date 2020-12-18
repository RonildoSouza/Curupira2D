using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.Common.Systems;
using MonoGame.Helper.ECS.Components.Drawables;

namespace MonoGame.Helper.Samples.Systems.SceneGraph
{
    [RequiredComponent(typeof(SpriteComponent))]
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
