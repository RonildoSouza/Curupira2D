using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;

namespace Helper.SceneGraph.Systems
{
    [RequiredComponent(typeof(SpriteComponent))]
    public class CharacterMovementSystem : MonoGame.Helper.ECS.System, IInitializable, IUpdatable
    {
        const float VELOCITY = 100f;
        Entity characterEntity;

        public void Initialize()
        {
            var characterTexture = Scene.GameCore.Content.Load<Texture2D>("character");

            characterEntity = Scene.CreateEntity("character")
                .SetPosition(400, 240)
                .AddComponent(new SpriteComponent(characterTexture));
        }

        public void Update()
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Left))
                HorizontalMove(ref characterEntity, false);

            if (ks.IsKeyDown(Keys.Up))
                VerticalMove(ref characterEntity, false);

            if (ks.IsKeyDown(Keys.Right))
                HorizontalMove(ref characterEntity);

            if (ks.IsKeyDown(Keys.Down))
                VerticalMove(ref characterEntity);
        }

        void HorizontalMove(ref Entity characterEntity, bool moveRight = true)
        {
            var spriteComponent = characterEntity.GetComponent<SpriteComponent>();
            var tempPosition = characterEntity.Transform.Position;
            var direction = moveRight ? 1 : -1;

            tempPosition.X += (float)(VELOCITY * Scene.DeltaTime) * direction;

            #region Out of screen in left or right
            if (tempPosition.X + spriteComponent.TextureSize.X < 0f)
                tempPosition.X = Scene.ScreenWidth;

            if (tempPosition.X > Scene.ScreenWidth)
                tempPosition.X = -spriteComponent.TextureSize.X;
            #endregion

            characterEntity.SetPosition(tempPosition);
        }

        void VerticalMove(ref Entity characterEntity, bool moveDown = true)
        {
            var spriteComponent = characterEntity.GetComponent<SpriteComponent>();
            var tempPosition = characterEntity.Transform.Position;
            var direction = moveDown ? 1 : -1;

            tempPosition.Y += (float)(VELOCITY * Scene.DeltaTime) * direction;

            #region Out of screen in top or bottom
            if (tempPosition.Y + spriteComponent.TextureSize.Y < 0f)
                tempPosition.Y = Scene.ScreenHeight;

            if (tempPosition.Y > Scene.ScreenHeight)
                tempPosition.Y = -spriteComponent.TextureSize.X;
            #endregion

            characterEntity.SetPosition(tempPosition);
        }
    }
}
