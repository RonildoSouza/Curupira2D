using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Samples.Common.Systems;

namespace Curupira2D.Samples.Systems.SpriteAnimation
{
    [RequiredComponent(typeof(CharacterMovementSystem), typeof(SpriteAnimationComponent))]
    class CharacterMovementSystem : EntityMovementSystemBase
    {
        protected override string EntityUniqueId => "character";
    }
}
