using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.Extensions;

namespace MonoGame.Helper.ECS.Systems
{
    [RequiredComponent(typeof(TextComponent))]
    public class TextSystem : System, IRenderable
    {
        public virtual void Draw()
        {
            SceneMatchEntitiesIteration(entity =>
            {
                var textComponent = entity.GetComponent<TextComponent>();
                Scene.SpriteBatch.DrawString(entity, textComponent);
            });
        }
    }
}
