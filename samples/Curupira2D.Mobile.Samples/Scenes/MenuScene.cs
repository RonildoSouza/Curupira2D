using Curupira2D.Mobile.Samples.Common.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using System;

namespace Curupira2D.Mobile.Samples.Scenes
{
    public class MenuScene : SceneBase
    {
        private Desktop _desktop;

        public MenuScene() : base(activeReturnButton: false) { }

        public override void LoadContent()
        {
            SetTitle(nameof(MenuScene));

            MyraEnvironment.Game = GameCore;

            var rootVerticalStackPanel = new VerticalStackPanel
            {
                Spacing = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            rootVerticalStackPanel.Widgets.Add(
                BuildMenuItem("S01 - JOYSTICK", (o, e) => GameCore.SetScene<S01JoystickScene>(), "UI/Images/S01JoystickScene"));
            rootVerticalStackPanel.Widgets.Add(
                BuildMenuItem("S02 - TOP DOWN CAR MOVEMENT", (o, e) => GameCore.SetScene<S02TopDownCarMovementScene>(), "UI/Images/S02TopDownCarMovementScene"));
            rootVerticalStackPanel.Widgets.Add(
                BuildMenuItem("S03 - ASTEROIDS MOVEMENT", (o, e) => GameCore.SetScene<S03AsteroidsMovementScene>(), "UI/Images/S03AsteroidsMovementScene"));

            _desktop = new Desktop { Root = rootVerticalStackPanel };

            base.LoadContent();
        }

        public override void Draw()
        {
            _desktop.Render();
            base.Draw();
        }

        public override void Dispose()
        {
            _desktop.Dispose();
            base.Dispose();
        }

        Button BuildMenuItem(string text, EventHandler click, string imagePath)
        {
            var heightItem = (int)(ScreenHeight / 3.5f);

            var buttonContentHorizontalStackPanel = new HorizontalStackPanel
            {
                Spacing = 10,
                Padding = new Myra.Graphics2D.Thickness(4),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
            };

            // Set image
            buttonContentHorizontalStackPanel.Widgets.Add(new Image
            {
                Width = (int)(ScreenWidth * 0.3f),
                Height = heightItem,
                Renderable = new TextureRegion(GameCore.Content.Load<Texture2D>(imagePath))
            });

            // Set label
            buttonContentHorizontalStackPanel.Widgets.Add(new Label
            {
                Scale = new Vector2(2),
                Margin = new Myra.Graphics2D.Thickness((int)(ScreenWidth * 0.16f), 0, 0, 0),
                Width = (int)(ScreenWidth * 0.7f),
                VerticalAlignment = VerticalAlignment.Center,
                Text = text
            });

            var button = new Button
            {
                Width = (int)(ScreenWidth * 0.9f),
                Height = heightItem,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Content = buttonContentHorizontalStackPanel,
            };

            button.Click += click;

            return button;
        }
    }
}
