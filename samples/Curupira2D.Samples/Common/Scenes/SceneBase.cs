using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Curupira2D.Testbed.Common.Scenes
{
    abstract class SceneBase : Scene
    {
        protected void ShowText(float x, float y, string text, Color color = default)
        {
            var fontArial = GameCore.Content.Load<SpriteFont>("FontArial");
            CreateEntity(Guid.NewGuid().ToString().Substring(0, 6))
                .SetPosition(x, y)
                .AddComponent(new TextComponent(
                    fontArial,
                    $"{text}",
                    color: color == default ? Color.DarkBlue : color,
                    layerDepth: 1));
        }

        protected void ShowControlTips(float x, float y, string text, Color color = default) =>
            ShowText(x, y, $"*QUIT: Key Q*\n\nCONTROLS\n{text}", color);
    }
}
