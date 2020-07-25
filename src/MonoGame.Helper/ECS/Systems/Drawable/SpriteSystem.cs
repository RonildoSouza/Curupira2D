using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.Extensions;

namespace MonoGame.Helper.ECS.Systems.Drawable
{
    [RequiredComponent(typeof(SpriteComponent))]
    public class SpriteSystem : System, IRenderable
    {
        public void Draw()
        {
            SceneMatchEntitiesIteration(entity =>
            {
                var spriteComponent = entity.GetComponent<SpriteComponent>();
                Scene.SpriteBatch.Draw(entity, spriteComponent);
            });
        }
    }
}
