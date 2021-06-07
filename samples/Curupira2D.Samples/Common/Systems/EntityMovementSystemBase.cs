using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Testbed.Common.Systems
{
    abstract class EntityMovementSystemBase : ECS.System, ILoadable, IUpdatable
    {
        protected Entity _entityToMove;
        protected Vector2 _entitySize;
        protected float Velocity { get; set; } = 100f;
        protected abstract string EntityUniqueId { get; }

        public virtual void LoadContent()
        {
            if (_entityToMove == null)
                _entityToMove = Scene.GetEntity(EntityUniqueId);

            var spriteComponent = _entityToMove.GetComponent<SpriteComponent>();
            var spriteAnimationComponent = _entityToMove.GetComponent<SpriteAnimationComponent>();

            if (spriteComponent != null)
                _entitySize = spriteComponent.TextureSize;
            else if (spriteAnimationComponent != null)
                _entitySize = new Vector2(spriteAnimationComponent.FrameWidth, spriteAnimationComponent.FrameHeight);
        }

        public void Update()
        {
            if (_entityToMove == null)
                return;

            var ks = Keyboard.GetState();
            var tempPosition = _entityToMove.Transform.Position;
            var direction = Vector2.Zero;

            if (ks.IsKeyDown(Keys.Left))
                direction.X -= 1;

            if (ks.IsKeyDown(Keys.Up))
                direction.Y -= 1;

            if (ks.IsKeyDown(Keys.Right))
                direction.X += 1;

            if (ks.IsKeyDown(Keys.Down))
                direction.Y += 1;

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
