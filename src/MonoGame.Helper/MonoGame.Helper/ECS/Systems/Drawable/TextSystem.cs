using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.Extensions;
using System.Linq;

namespace MonoGame.Helper.ECS.Systems
{
    [RequiredComponent(typeof(TextComponent))]
    public class TextSystem : System, IRenderable
    {
        public virtual void Draw()
        {
            var entities = Scene.GetEntities(_ => Matches(_));

            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities.ElementAt(i);
                var textComponent = entity.GetComponent<TextComponent>();

                Scene.SpriteBatch.DrawString(entity, textComponent);
            }
        }
    }
}
