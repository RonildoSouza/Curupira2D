using MonoGame.Helper.Common.Systems;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems.Attributes;

namespace MonoGame.Helper.Samples.Systems.SpriteAnimation
{
    [RequiredComponent(typeof(CharacterMovementSystem), typeof(SpriteAnimationComponent))]
    class CharacterMovementSystem : EntityMovementSystemBase
    {
        protected override string EntityUniqueId => "character";
    }
}
