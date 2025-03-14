using AssetManagementBase;
using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.Desktop.Samples.Scenes.TiledMap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using System.IO;

namespace Curupira2D.Desktop.Samples.Scenes
{
    class MenuScene : SceneBase
    {
        private Myra.Graphics2D.UI.Desktop _desktop;

        public override void LoadContent()
        {
            SetTitle(nameof(MenuScene));

            GameCore.IsMouseVisible = true;
            MyraEnvironment.Game = GameCore;

            var assetManager = AssetManager.CreateFileAssetManager($"{GameCore.Content.RootDirectory}/UI");
            var project = Project.LoadFromXml(File.ReadAllText($"{GameCore.Content.RootDirectory}/UI/Menu.xmmp"), assetManager);

            project.Root.FindChildById<Button>("SpriteAnimationButton").Click += (o, e) => GameCore.SetScene<SpriteAnimationScene>();
            project.Root.FindChildById<Button>("SceneGraphButton").Click += (o, e) => GameCore.SetScene<SceneGraphScene>();
            project.Root.FindChildById<Button>("CameraButton").Click += (o, e) => GameCore.SetScene<CameraScene>();
            project.Root.FindChildById<Button>("PhysicButton").Click += (o, e) => GameCore.SetScene<PhysicScene>();
            project.Root.FindChildById<Button>("PlatformerTiledMapButton").Click += (o, e) => GameCore.SetScene<PlatformerTiledMapScene>();
            project.Root.FindChildById<Button>("AetherPhysics2DHelloWorldButton").Click += (o, e) => GameCore.SetScene<AetherPhysics2DHelloWorldScene>();
            project.Root.FindChildById<Button>("QuadtreeCheckCollisionButton").Click += (o, e) => GameCore.SetScene<QuadtreeCheckCollisionScene>();
            project.Root.FindChildById<Button>("MassivelyMultiplayerOnlineButton").Click += (o, e) => GameCore.SetScene<MassivelyMultiplayerOnlineScene>();
            project.Root.FindChildById<Button>("TiledMapWithManyLayersButton").Click += (o, e) => GameCore.SetScene<TiledMapWithManyLayersScene>();
            project.Root.FindChildById<Button>("IsometricTiledMapButton").Click += (o, e) => GameCore.SetScene<IsometricTiledMapScene>();
            project.Root.FindChildById<Button>("IsometricGravityTiledMapButton").Click += (o, e) => GameCore.SetScene<IsometricGravityTiledMapScene>();

            _desktop = new Myra.Graphics2D.UI.Desktop { Root = project.Root };

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardInputManager.Begin();

            if (KeyboardInputManager.IsKeyPressed(Keys.T))
                GameCore.SetScene<SceneTest>();

            KeyboardInputManager.End();

            base.Update(gameTime);
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
    }
}
