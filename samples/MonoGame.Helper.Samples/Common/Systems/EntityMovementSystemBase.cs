using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;

namespace MonoGame.Helper.Common.Systems
{
    abstract class EntityMovementSystemBase : ECS.System, IInitializable, IUpdatable
    {
        protected Entity _entityToMove;
        protected Vector2 _entitySize;
        protected float Velocity { get; set; } = 100f;
        protected abstract string EntityUniqueId { get; }

        public virtual void Initialize()
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

            if (ks.IsKeyDown(Keys.Left))
                HorizontalMove(false);

            if (ks.IsKeyDown(Keys.Up))
                VerticalMove(false);

            if (ks.IsKeyDown(Keys.Right))
                HorizontalMove();

            if (ks.IsKeyDown(Keys.Down))
                VerticalMove();
        }

        void HorizontalMove(bool moveRight = true)
        {
            var tempPosition = _entityToMove.Transform.Position;
            var direction = moveRight ? 1 : -1;

            tempPosition.X += (float)(Velocity * Scene.DeltaTime) * direction;

            #region Out of screen in left or right
            if (tempPosition.X + _entitySize.X < 0f)
                tempPosition.X = Scene.ScreenWidth;

            if (tempPosition.X > Scene.ScreenWidth)
                tempPosition.X = -_entitySize.X;
            #endregion

            _entityToMove.SetPosition(tempPosition);
        }

        void VerticalMove(bool moveDown = true)
        {
            var tempPosition = _entityToMove.Transform.Position;
            var direction = moveDown ? 1 : -1;

            tempPosition.Y += (float)(Velocity * Scene.DeltaTime) * direction;

            #region Out of screen in top or bottom
            if (tempPosition.Y + _entitySize.Y < 0f)
                tempPosition.Y = Scene.ScreenHeight;

            if (tempPosition.Y > Scene.ScreenHeight)
                tempPosition.Y = -_entitySize.Y;
            #endregion

            _entityToMove.SetPosition(tempPosition);
        }
    }
}
