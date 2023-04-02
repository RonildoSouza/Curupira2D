using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.Extensions;
using Curupira2D.Mobile.Samples.Common.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.Mobile.Samples.Scenes
{
    public class MenuScene : SceneBase
    {
        IList<Entity> _itemsEntity;

        public MenuScene() : base(activeReturnButton: false) { }

        public override void LoadContent()
        {
            SetTitle(nameof(MenuScene));

            var spriteFont = GameCore.Content.Load<SpriteFont>("Common/FontImpact18");
            _itemsEntity = new List<Entity>
            {
                CreateEntity("S01", Vector2.Zero)
                    .AddComponent(new TextComponent(
                        spriteFont,
                        "S01 - JOYSTICK",
                        color: Color.Black,
                        scale: new Vector2(3.5f))),
                CreateEntity("S02", Vector2.Zero)
                    .AddComponent(new TextComponent(
                        spriteFont,
                        "S02 - TOP DOWN CAR MOVEMENT",
                        color: Color.Black,
                        scale: new Vector2(3.5f))),
                CreateEntity("S03", Vector2.Zero)
                    .AddComponent(new TextComponent(
                        spriteFont,
                        "S03 - ASTEROIDS MOVEMENT",
                        color: Color.Black,
                        scale: new Vector2(3.5f))),
            };

            var itemHeightSize = ScreenHeight / _itemsEntity.Count;
            for (int i = 0; i < _itemsEntity.Count; i++)
            {
                var itemEntity = _itemsEntity[i];
                itemEntity.SetPosition(ScreenCenter.X, InvertPositionY(itemHeightSize * (i + 0.5f)));
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var itemEntity = _itemsEntity.FirstOrDefault(
                _ => TouchLocationRectangle.Intersects(_.GetHitBox()) && TouchLocation.State == TouchLocationState.Released);

            switch (itemEntity?.UniqueId)
            {
                case "S01":
                    GameCore.SetScene<S01JoystickScene>();
                    break;
                case "S02":
                    GameCore.SetScene<S02TopDownCarMovementScene>();
                    break;
                case "S03":
                    GameCore.SetScene<S03AsteroidsMovementScene>();
                    break;
            }

            base.Update(gameTime);
        }
    }
}
