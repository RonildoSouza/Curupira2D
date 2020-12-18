using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Components.Drawables;

namespace MonoGame.Helper.Samples.Systems.SpriteAnimation
{
    [RequiredComponent(typeof(SpriteAnimationComponent))]
    class CharacterMovementSystem : EntityMovementSystemBase
    {
        protected override string EntityUniqueId => "character";
    }
}
