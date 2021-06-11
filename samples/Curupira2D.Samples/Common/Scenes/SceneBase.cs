using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Curupira2D.Samples.Common.Scenes
{
    abstract class SceneBase : Scene
    {
        protected void ShowText(string text, float? x = null, float? y = null, Color color = default)
        {
            var fontArial = GameCore.Content.Load<SpriteFont>("FontArial");
            var textComponent = new TextComponent(
                    fontArial,
                    $"{text}",
                    color: color == default ? Color.DarkBlue : color,
                    layerDepth: 1);

            CreateEntity(Guid.NewGuid().ToString().Substring(0, 6))
                .SetPosition(
                    x ?? ScreenWidth * 0.2f,
                    y ?? ScreenHeight - textComponent.TextSize.Y)
                .AddComponent(textComponent);
        }

        protected void ShowControlTips(string text, float? x = null, float? y = null, Color color = default) =>
            ShowText($"*QUIT: Key Q*\n\nCONTROLS\n{text}", x, y, color);
    }
}
