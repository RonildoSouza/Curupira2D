using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems.Attributes;
using MonoGame.Helper.Extensions;
using System.Collections.Generic;

namespace MonoGame.Helper.ECS.Systems.Drawables
{
    [RequiredComponent(typeof(SpriteSystem), typeof(SpriteComponent))]
    public class SpriteSystem : DrawableSystem<SpriteComponent>
    {
        protected override void DrawEntities(ref IReadOnlyList<Entity> entities)
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
