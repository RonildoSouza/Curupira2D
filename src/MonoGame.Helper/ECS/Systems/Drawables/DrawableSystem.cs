using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper.ECS.Components.Drawables;
using System.Collections.Generic;

namespace MonoGame.Helper.ECS.Systems.Drawables
{
    public abstract class DrawableSystem<TDrawableComponent> : System, IRenderable
        where TDrawableComponent : DrawableComponent
    {
        public void Draw()
        {
            DrawFixedPositionEntities();
            DrawNonFixedPositionEntities();
        }

        protected void DrawFixedPositionEntities()
        {
            Scene.SpriteBatch.Begin(SpriteSortMode.BackToFront);

            var fixedPositionEntities = Scene.GetEntities(_ =>
            {
                var drawableComponent = _.GetComponent<TDrawableComponent>();
                return MatchActiveEntitiesAndComponents(_) && drawableComponent.FixedPosition;
            });

            DrawEntities(ref fixedPositionEntities);

            Scene.SpriteBatch.End();
        }

        protected void DrawNonFixedPositionEntities()
        {
            Scene.SpriteBatch.Begin(
                sortMode: SpriteSortMode.BackToFront,
                blendState: BlendState.AlphaBlend,
                transformMatrix: Scene.Camera2D.TransformationMatrix);

            var nonFixedPositionEntities = Scene.GetEntities(_ =>
            {
                var drawableComponent = _.GetComponent<TDrawableComponent>();
                return MatchActiveEntitiesAndComponents(_) && !drawableComponent.FixedPosition;
            });

            DrawEntities(ref nonFixedPositionEntities);

            Scene.SpriteBatch.End();
        }

        protected abstract void DrawEntities(ref IReadOnlyList<Entity> entities);
    }
}
