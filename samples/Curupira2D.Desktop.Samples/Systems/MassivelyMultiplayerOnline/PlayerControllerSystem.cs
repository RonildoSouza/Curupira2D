using Curupira2D.Desktop.Samples.Common.Systems;
using Curupira2D.Desktop.Samples.Models.MassivelyMultiplayerOnline;
using Curupira2D.Desktop.Samples.Scenes;
using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;

namespace Curupira2D.Desktop.Samples.Systems.MassivelyMultiplayerOnline
{
    [RequiredComponent(typeof(PlayerControllerSystem), typeof(SpriteComponent))]
    class PlayerControllerSystem : EntityMovementSystemBase, ILoadable, IUpdatable
    {
        MassivelyMultiplayerOnlineScene _mmoScene;
        string _playerUniqueId;
        Entity _playerEntity;
        Texture2D _playerTexture2D;
        Color _playerColor;
        (Entity, Entity) _playerEyesEntities;
        (Texture2D, Texture2D) _playerEyesTexture2D;
        bool _send_message = true;

        protected override string EntityUniqueId => _playerUniqueId;

        public override void LoadContent()
        {
            _mmoScene = (MassivelyMultiplayerOnlineScene)Scene;
            _playerUniqueId = $"player_{Guid.NewGuid()}";

            _playerEyesTexture2D = (
                Scene.GameCore.GraphicsDevice.CreateTextureRectangle(Random.Shared.Next(5, 15), Random.Shared.Next(5, 20), Color.Black),
                Scene.GameCore.GraphicsDevice.CreateTextureRectangle(Random.Shared.Next(5, 15), Random.Shared.Next(5, 20), Color.Black)
            );

            _playerColor = new Color(Random.Shared.Next(0, 256), Random.Shared.Next(0, 256), Random.Shared.Next(0, 256));
            _playerTexture2D = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(Random.Shared.Next(25, 50), Random.Shared.Next(50, 75), _playerColor);

            var safeZoneW = Random.Shared.Next(6, 10) / 10.0f;
            var safeZoneH = Random.Shared.Next(7, 10) / 10.0f;

            _playerEntity = Scene.CreateEntity(_playerUniqueId, Scene.ScreenWidth * safeZoneW, Scene.ScreenHeight * safeZoneH).AddComponent(new SpriteComponent(_playerTexture2D));

            _playerEyesEntities = (
                Scene.CreateEntity($"L_eye_{_playerEntity.UniqueId}", Vector2.Zero).AddComponent(new SpriteComponent(_playerEyesTexture2D.Item1)),
                Scene.CreateEntity($"R_eye_{_playerEntity.UniqueId}", Vector2.Zero).AddComponent(new SpriteComponent(_playerEyesTexture2D.Item2))
            );

            _playerEntity.AddChild(_playerEyesEntities.Item1).AddChild(_playerEyesEntities.Item2);

            UpdatePlayerEyesPositions();

            SendPlayerData(EnemyDataType.Joined);

            base.LoadContent();
        }

        public override void Update()
        {
            base.Update();

            UpdatePlayerEyesPositions();

            if (_playerEntity.IsCollidedWithAny(Scene, "enemy"))
                Scene.SetCleanColor(Color.MonoGameOrange);
            else
                Scene.SetFallbackCleanColor();

            if (_send_message)
            {
                SendPlayerData(EnemyDataType.Message);
                _send_message = false;
            }

            _send_message = ChangePosition;
        }

        public override void OnRemoveFromScene()
        {
            SendPlayerData(EnemyDataType.Left);
            base.OnRemoveFromScene();
        }

        private void UpdatePlayerEyesPositions()
        {
            var lEyePosX = _playerEntity.Position.X - _playerTexture2D.Width * 0.25f;
            var rEyePosX = _playerEntity.Position.X + _playerTexture2D.Width * 0.25f;

            var eyePosY = _playerEntity.Position.Y + _playerTexture2D.Height * 0.25f;

            _playerEyesEntities.Item1.SetPosition(lEyePosX, eyePosY);
            _playerEyesEntities.Item2.SetPosition(rEyePosX, eyePosY);

            _playerEyesEntities.Item1.Position.GetSafeNormalize();
            _playerEyesEntities.Item2.Position.GetSafeNormalize();
        }

        private void SendPlayerData(EnemyDataType enemyDataType)
        {
            _mmoScene.WSSendMessage(JsonConvert.SerializeObject(
                new EnemyData(
                    enemyDataType,
                    _playerEntity.UniqueId,
                    _playerEntity.Position,
                    _playerColor,
                    new Point(_playerTexture2D.Width, _playerTexture2D.Height),
                    (
                        new Point(_playerEyesTexture2D.Item1.Width, _playerEyesTexture2D.Item1.Height),
                        new Point(_playerEyesTexture2D.Item2.Width, _playerEyesTexture2D.Item2.Height)
                    )
                )
            ));
        }
    }
}
