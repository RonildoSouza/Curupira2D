using Curupira2D.Desktop.Samples.Components.SceneGraph;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Desktop.Samples.Systems.SceneGraph
{
    [RequiredComponent(typeof(EquipmentMovimentSystem), typeof(EquipmentComponent))]
    class EquipmentMovimentSystem : ECS.System, ILoadable, IUpdatable
    {
        public void LoadContent()
        {
            var hatTexture = Scene.GameCore.Content.Load<Texture2D>("SceneGraph/hat");
            var staffTexture = Scene.GameCore.Content.Load<Texture2D>("SceneGraph/staff");

            var hatEntity = Scene.CreateEntity("hat", default)
                .AddComponent(new EquipmentComponent(0f, -90f))
                .AddComponent(new SpriteComponent(hatTexture));

            var staffEntity = Scene.CreateEntity("staff", default)
                .AddComponent(new EquipmentComponent(-80f, 10f))
                .AddComponent(new SpriteComponent(staffTexture));

            var characterEntity = Scene.GetEntity("character");
            characterEntity.AddChild(hatEntity);
            characterEntity.AddChild(staffEntity);
        }

        public void Update()
        {
            var entities = Scene.GetEntities(_ => MatchComponents(_));

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];

                var equipmentComponent = entity.GetComponent<EquipmentComponent>();
                var newPosition = entity.Parent.Position - equipmentComponent.OffsetPosition;

                entity.SetPosition(newPosition);

                if (entity.UniqueId == "hat" && Scene.KeyboardInputManager.IsKeyPressed(Keys.D1))
                    entity.SetActive(!entity.Active);

                if (entity.UniqueId == "staff" && Scene.KeyboardInputManager.IsKeyPressed(Keys.D2))
                    entity.SetActive(!entity.Active);
            }
        }
    }
}
