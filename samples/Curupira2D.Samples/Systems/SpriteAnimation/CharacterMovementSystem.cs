using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Curupira2D.Samples.Common.Systems;
using Microsoft.Xna.Framework;

namespace Curupira2D.Samples.Systems.SpriteAnimation
{
    [RequiredComponent(typeof(CharacterMovementSystem), typeof(SpriteAnimationComponent))]
    class CharacterMovementSystem : EntityMovementSystemBase
    {
        protected override string EntityUniqueId => "character";

        public override void Update()
        {
            if (_entityToMove.IsCollidedWith(Scene, "explosion"))
                Scene.SetCleanColor(Color.OrangeRed);
            else
                Scene.SetFallbackCleanColor();

            base.Update();
        }
    }
}
