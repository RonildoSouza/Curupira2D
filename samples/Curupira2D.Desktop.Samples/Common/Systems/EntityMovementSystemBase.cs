using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Desktop.Samples.Common.Systems
{
    abstract class EntityMovementSystemBase : ECS.System, ILoadable, IUpdatable
    {
        protected Entity _entityToMove;
        protected Vector2 _entitySize;
        protected float Velocity { get; set; } = 100f;
        protected bool ChangePosition { get; set; }
        protected abstract string EntityUniqueId { get; }

        public virtual void LoadContent()
        {
            _entityToMove ??= Scene.GetEntity(EntityUniqueId);

            var spriteComponent = _entityToMove.GetComponent<SpriteComponent>();
            var spriteAnimationComponent = _entityToMove.GetComponent<SpriteAnimationComponent>();

            if (spriteComponent != null)
                _entitySize = spriteComponent.TextureSize;
            else if (spriteAnimationComponent != null)
                _entitySize = new Vector2(spriteAnimationComponent.FrameWidth, spriteAnimationComponent.FrameHeight);
        }

        public virtual void Update()
        {
            if (_entityToMove == null)
                return;

            var tempPosition = _entityToMove.Position;
            var direction = Vector2.Zero;
            ChangePosition = false;

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Left) || Scene.KeyboardInputManager.IsKeyDown(Keys.A))
            {
                ChangePosition = true;
                direction.X -= 1;
            }

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Up) || Scene.KeyboardInputManager.IsKeyDown(Keys.W))
            {
                ChangePosition = true;
                direction.Y += 1;
            }

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Right) || Scene.KeyboardInputManager.IsKeyDown(Keys.D))
            {
                ChangePosition = true;
                direction.X += 1;
            }

            if (Scene.KeyboardInputManager.IsKeyDown(Keys.Down) || Scene.KeyboardInputManager.IsKeyDown(Keys.S))
            {
                ChangePosition = true;
                direction.Y -= 1;
            }

            tempPosition += (float)(Velocity * Scene.DeltaTime) * direction.GetSafeNormalize();

            #region Out of screen in left, right, top or bottom
            if (tempPosition.X + _entitySize.X < 0f)
                tempPosition.X = Scene.ScreenWidth;

            if (tempPosition.X > Scene.ScreenWidth)
                tempPosition.X = -_entitySize.X;

            if (tempPosition.Y + _entitySize.Y < 0f)
                tempPosition.Y = Scene.ScreenHeight;

            if (tempPosition.Y > Scene.ScreenHeight)
                tempPosition.Y = -_entitySize.Y;
            #endregion 

            _entityToMove.SetPosition(tempPosition);
        }
    }
}
