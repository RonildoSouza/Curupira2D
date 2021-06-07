using Curupira2D.ECS.Components.Drawables;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Curupira2D.ECS.Systems.Drawables
{
    public abstract class DrawableSystem<TDrawableComponent> : System, IRenderable
        where TDrawableComponent : DrawableComponent
    {
        public void Draw()
        {
            DrawWithoutUsingCameraEntities();
            DrawUsingCameraEntities();
        }

        protected void DrawWithoutUsingCameraEntities()
        {
            Scene.SpriteBatch.Begin(SpriteSortMode.BackToFront);

            var withoutUsingCameraEntities = Scene.GetEntities(_ =>
            {
                var drawableComponent = _.GetComponent<TDrawableComponent>();
                return MatchActiveEntitiesAndComponents(_) && drawableComponent.DrawWithoutUsingCamera;
            });

            DrawEntities(ref withoutUsingCameraEntities);

            Scene.SpriteBatch.End();
        }

        protected void DrawUsingCameraEntities()
        {
            Scene.SpriteBatch.Begin(
                sortMode: SpriteSortMode.BackToFront,
                rasterizerState: RasterizerState.CullClockwise,
                effect: Scene.Camera2D.SpriteBatchEffect);

            var usingCameraEntities = Scene.GetEntities(_ =>
            {
                var drawableComponent = _.GetComponent<TDrawableComponent>();
                return MatchActiveEntitiesAndComponents(_) && !drawableComponent.DrawWithoutUsingCamera;
            });

            DrawEntities(ref usingCameraEntities);

            Scene.SpriteBatch.End();
        }

        protected abstract void DrawEntities(ref IReadOnlyList<Entity> entities);
    }
}
