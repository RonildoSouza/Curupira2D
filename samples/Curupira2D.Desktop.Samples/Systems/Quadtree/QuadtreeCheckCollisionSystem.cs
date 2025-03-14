using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.Desktop.Samples.Common.Systems;
using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace Curupira2D.Desktop.Samples.Systems.Quadtree
{
    [RequiredComponent(typeof(QuadtreeCheckCollisionSystem), typeof(SpriteComponent))]
    class QuadtreeCheckCollisionSystem : EntityMovementSystemBase
    {
        private Entity _playerEntity;
        private TextComponent _textComponent, _playerTextComponent;
        private static readonly Random _random = new Random();

        protected override string EntityUniqueId => "player";

        public override void LoadContent()
        {
            var playerTexture = Scene.GameCore.Content.Load<Texture2D>("Quadtree/green-square");
            var enemyTexture = Scene.GameCore.Content.Load<Texture2D>("Quadtree/red-square");
            var fontArial = Scene.GameCore.Content.Load<SpriteFont>("FontArial");

            _playerTextComponent = new TextComponent(fontArial, string.Empty, color: Color.Black, drawInUICamera: false, layerDepth: 1f, scale: new Vector2(0.5f));
            _playerEntity = Scene.CreateEntity(EntityUniqueId, Scene.ScreenCenter)
                .AddComponent(new SpriteComponent(playerTexture))
                .AddComponent(_playerTextComponent);

            for (int i = 1; i <= 30; i++)
            {
                var uniqueId = $"{i}";

                var enemyEntity = Scene.CreateEntity(uniqueId, default)
                    .AddComponent(new SpriteComponent(enemyTexture))
                    .AddComponent(new TextComponent(fontArial, uniqueId, color: Color.White, drawInUICamera: false, layerDepth: 1f, scale: new Vector2(0.5f)));

                SetEnemyPosition(enemyEntity, enemyTexture.Width, enemyTexture.Height);
            }

            _textComponent = (Scene as SceneBase).ShowText($"ENEMIES TO COLLISION:", Scene.ScreenWidth * 0.8f, Scene.ScreenCenter.Y, color: Color.Black);

            base.LoadContent();
        }

        public override void Update()
        {
            if (_playerEntity.IsCollidedWithAny(Scene))
                Scene.SetCleanColor(Color.MonoGameOrange);
            else
                Scene.SetFallbackCleanColor();

            var returnObjects = Scene.Quadtree.Retrieve(_playerEntity);
            _textComponent.Text = $"ENEMIES TO COLLISION:\n\n" +
                $"{string.Join("\n", returnObjects.Select(_ => $"Enemy: {_.UniqueId} | {_.Position}"))}";

            _playerTextComponent.Text = $"{Vector2.Round(_playerEntity.Position)}";
            _playerTextComponent.Position = new Vector2(_playerEntity.Position.X, _playerEntity.Position.Y + 32f);

            base.Update();
        }

        private void SetEnemyPosition(Entity enemyEntity, int enemyWidth, int enemyHeight)
        {
            var x = _random.Next(enemyWidth, Scene.ScreenWidth - enemyWidth);
            var y = _random.Next(enemyHeight, Scene.ScreenHeight - enemyHeight);

            enemyEntity.SetPosition(x, y);

            if (enemyEntity.IsCollidedWithAny(Scene))
                SetEnemyPosition(enemyEntity, enemyWidth, enemyHeight);
        }
    }
}
