using Curupira2D.Desktop.Samples.Models.MassivelyMultiplayerOnline;
using Curupira2D.Desktop.Samples.Scenes;
using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Curupira2D.Desktop.Samples.Systems.MassivelyMultiplayerOnline
{
    [RequiredComponent(typeof(EnemiesControllerSystem), typeof(SpriteComponent))]
    class EnemiesControllerSystem : ECS.System, ILoadable
    {
        public void LoadContent()
        {
            var mmoScene = (MassivelyMultiplayerOnlineScene)Scene;

            mmoScene.WSClient.Subscribe(msg =>
            {
                var enemyData = JsonConvert.DeserializeObject<EnemyData>(msg.Text);

                switch (enemyData?.Type)
                {
                    case EnemyDataType.Joined:
                        CreateEnemyEntity(enemyData);
                        break;
                    case EnemyDataType.Left:
                        Scene.RemoveEntity(enemyData.UniqueId);
                        break;
                    case EnemyDataType.Message:
                        CreateEnemyEntity(enemyData);
                        UpdateEnemyPositions(enemyData.UniqueId, enemyData);
                        break;
                }
            });
        }

        private void CreateEnemyEntity(EnemyData enemyData)
        {
            if (Scene.ExistsEntities(enemyData?.UniqueId))
                return;

            var enemyEyesTexture2D = (
                Scene.GameCore.GraphicsDevice.CreateTextureRectangle(enemyData.EyesTextureSize.Item1, Color.Black),
                Scene.GameCore.GraphicsDevice.CreateTextureRectangle(enemyData.EyesTextureSize.Item2, Color.Black)
            );

            var enemyTexture2D = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(enemyData.TextureSize, enemyData.Color);

            var enemyEntity = Scene.CreateEntity(enemyData.UniqueId, enemyData.Position, "enemy").AddComponent(new SpriteComponent(enemyTexture2D));

            var enemyEyesEntities = (
                Scene.CreateEntity($"L_eye_{enemyEntity.UniqueId}", Vector2.Zero).AddComponent(new SpriteComponent(enemyEyesTexture2D.Item1)),
                Scene.CreateEntity($"R_eye_{enemyEntity.UniqueId}", Vector2.Zero).AddComponent(new SpriteComponent(enemyEyesTexture2D.Item2))
            );

            enemyEntity.AddChild(enemyEyesEntities.Item1).AddChild(enemyEyesEntities.Item2);
        }

        private void UpdateEnemyPositions(string enemyUniqueId, EnemyData enemyData, (Entity, Entity)? enemyEyesEntities = null)
        {
            var enemyEntity = Scene.GetEntity(enemyUniqueId);

            if (enemyEntity == null)
                return;

            enemyEntity.SetPosition(enemyData.Position);

            var enemySpriteComponent = enemyEntity.GetComponent<SpriteComponent>();

            var lEyePosX = enemyEntity.Position.X - enemySpriteComponent.TextureSize.X * 0.25f;
            var rEyePosX = enemyEntity.Position.X + enemySpriteComponent.TextureSize.X * 0.25f;

            var eyePosY = enemyEntity.Position.Y + enemySpriteComponent.TextureSize.Y * 0.25f;

            enemyEyesEntities ??= (
                Scene.GetEntity($"L_eye_{enemyEntity.UniqueId}"),
                Scene.GetEntity($"R_eye_{enemyEntity.UniqueId}")
            );

            enemyEyesEntities?.Item1.SetPosition(lEyePosX, eyePosY);
            enemyEyesEntities?.Item2.SetPosition(rEyePosX, eyePosY);

            enemyEyesEntities?.Item1.Position.GetSafeNormalize();
            enemyEyesEntities?.Item2.Position.GetSafeNormalize();
        }
    }
}
