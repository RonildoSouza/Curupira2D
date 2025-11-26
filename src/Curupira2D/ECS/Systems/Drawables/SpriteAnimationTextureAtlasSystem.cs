using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.ECS.Systems.Drawables
{
    [RequiredComponent(typeof(SpriteAnimationTextureAtlas), typeof(SpriteAnimationTextureAtlasComponent))]
    public sealed class SpriteAnimationTextureAtlas : System, IRenderable
    {
        public void Draw(ref IReadOnlyCollection<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities.ElementAt(i);
                var spriteAnimationComponent = entity.GetComponent<SpriteAnimationTextureAtlasComponent>();

                Animate(ref spriteAnimationComponent);
                Scene.SpriteBatch.Draw(entity, spriteAnimationComponent);
            }
        }

        void Animate(ref SpriteAnimationTextureAtlasComponent spriteAnimationComponent)
        {
            //if (spriteAnimationComponent.SourceRectangle == Rectangle.Empty || spriteAnimationComponent.SourceRectangle.Value.Width == 0 || spriteAnimationComponent.SourceRectangle.Value.Height == 0)
            //    throw new ArgumentException($"The argument {nameof(spriteAnimationComponent.SourceRectangle)} cannot be Empty or Width or Height be equals Zero!");

            if (!spriteAnimationComponent.IsPlaying)
                return;

            spriteAnimationComponent.ElapsedTime += (Scene.GameTime?.ElapsedGameTime).GetValueOrDefault();

            if (spriteAnimationComponent.ElapsedTime >= spriteAnimationComponent.FrameTime)
            {
                spriteAnimationComponent.CurrentTextureAtlasIndex++;

                if (spriteAnimationComponent.CurrentTextureAtlasIndex > spriteAnimationComponent.TotalTextureAtlases)
                {
                    spriteAnimationComponent.CurrentTextureAtlasIndex = 0;
                    spriteAnimationComponent.IsPlaying = spriteAnimationComponent.IsLooping;
                }

                var currentTextureAtlas = spriteAnimationComponent.TextureAtlases[spriteAnimationComponent.CurrentTextureAtlasIndex];
                spriteAnimationComponent.SourceRectangle = currentTextureAtlas.Frame.ToRectangle();

                spriteAnimationComponent.ElapsedTime = TimeSpan.Zero;
            }
        }
    }
}