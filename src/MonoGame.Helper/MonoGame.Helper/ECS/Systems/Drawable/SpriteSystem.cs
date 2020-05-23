using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.Extensions;
using System.Linq;

namespace MonoGame.Helper.ECS.Systems.Drawable
{
    [RequiredComponent(typeof(SpriteComponent))]
    public class SpriteSystem : System, IRenderable
    {
        public void Draw()
        {
            var entities = Scene.GetEntities(_ => Matches(_));

            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities.ElementAt(i);
                var spriteComponent = entity.GetComponent<SpriteComponent>();

                Scene.SpriteBatch.Draw(entity.Transform.Position, entity.Transform.RotationInDegrees, spriteComponent);
            }
        }
    }
}
