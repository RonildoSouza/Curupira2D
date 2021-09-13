using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using System.Collections.Generic;

namespace Curupira2D.ECS.Systems.Drawables
{
    [RequiredComponent(typeof(SpriteSystem), typeof(SpriteComponent))]
    public sealed class SpriteSystem : System, IRenderable
    {
        public void Draw(ref IReadOnlyList<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var spriteComponent = entity.GetComponent<SpriteComponent>();

                Scene.SpriteBatch.Draw(entity, spriteComponent);
            }
        }
    }
}
