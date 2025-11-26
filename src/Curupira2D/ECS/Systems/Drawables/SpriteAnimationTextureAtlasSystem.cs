using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.ECS.Systems.Drawables
{
    [RequiredComponent(typeof(SpriteAnimationTextureAtlasSystem), typeof(SpriteAnimationTextureAtlasComponent))]
    public sealed class SpriteAnimationTextureAtlasSystem : System, IRenderable
    {
        public void Draw(ref IReadOnlyCollection<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities.ElementAt(i);
                var spriteAnimationComponent = entity.GetComponent<SpriteAnimationTextureAtlasComponent>();

                if (!spriteAnimationComponent.IsPlaying)
                    return;

                spriteAnimationComponent.ElapsedTime += (Scene.GameTime?.ElapsedGameTime).GetValueOrDefault();

                if (spriteAnimationComponent.ElapsedTime >= spriteAnimationComponent.FrameTime)
                {
                    if (spriteAnimationComponent.CurrentTextureAtlasIndex > spriteAnimationComponent.TextureAtlases.Count - 1)
                    {
                        spriteAnimationComponent.CurrentTextureAtlasIndex = 0;
                        spriteAnimationComponent.IsPlaying = spriteAnimationComponent.IsLooping;
                    }

                    var currentTextureAtlas = spriteAnimationComponent.TextureAtlases[spriteAnimationComponent.CurrentTextureAtlasIndex];
                    spriteAnimationComponent.SourceRectangle = currentTextureAtlas.Frame.ToRectangle();
                    spriteAnimationComponent.Origin = currentTextureAtlas.Pivot.ToVector2();
                    spriteAnimationComponent.CurrentTextureAtlas = currentTextureAtlas;

                    spriteAnimationComponent.CurrentTextureAtlasIndex++;
                    spriteAnimationComponent.ElapsedTime = TimeSpan.Zero;
                }

                Scene.SpriteBatch.Draw(entity, spriteAnimationComponent);
            }
        }
    }
}