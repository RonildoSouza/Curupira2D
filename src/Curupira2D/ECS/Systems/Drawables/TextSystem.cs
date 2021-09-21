using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Curupira2D.ECS.Systems.Drawables
{
    [RequiredComponent(typeof(TextSystem), typeof(TextComponent))]
    public sealed class TextSystem : System, IRenderable
    {
        public void Draw(ref IReadOnlyList<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                var textComponents = entity.GetComponents<TextComponent>();

                Parallel.ForEach(textComponents, _ => Scene.SpriteBatch.DrawString(entity, _));
            }
        }
    }
}
