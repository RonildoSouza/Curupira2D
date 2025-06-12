using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.ECS.Systems.Drawables
{
    [RequiredComponent(typeof(TextSystem), typeof(TextComponent))]
    public sealed class TextSystem : System, IRenderable
    {
        public void Draw(ref IReadOnlyCollection<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities.ElementAt(i);
                var textComponent = entity.GetComponent<TextComponent>();

                Scene.SpriteBatch.DrawString(entity, textComponent);
            }
        }
    }
}
