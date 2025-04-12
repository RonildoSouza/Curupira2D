using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Curupira2D.Desktop.Samples.Common.Scenes
{
    abstract class SceneBase : Scene
    {
        public TextComponent ShowText(string text, float? x = null, float? y = null, Color? color = null, Vector2 scale = default)
        {
            if (scale == default)
                scale = new Vector2(0.5f);

            var fontArial = GameCore.Content.Load<SpriteFont>("FontArial");
            var textComponent = new TextComponent(fontArial, $"{text}", color: color ?? Color.DarkBlue, layerDepth: 1f, scale: scale);
            var posX = x ?? ScreenWidth * 0.23f;
            var posY = y ?? ScreenHeight - textComponent.TextSize.Y * scale.Y;

            CreateEntity(Guid.NewGuid().ToString().Substring(0, 6), posX, posY, isCollidable: false)
                .AddComponent(textComponent);

            return textComponent;
        }

        protected void ShowControlTips(string text, float? x = null, float? y = null, Color? color = null, Vector2 scale = default) =>
            ShowText($"*RETURN TO MENU: Key Q or Backspace*\n\nCONTROLS\n{text}", x, y, color, scale);
    }
}
