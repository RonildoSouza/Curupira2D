using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Testbed.Components.SceneGraph;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Testbed.Systems.SceneGraph
{
    [RequiredComponent(typeof(EquipmentMovimentSystem), typeof(EquipmentComponent))]
    class EquipmentMovimentSystem : ECS.System, IInitializable, IUpdatable
    {
        KeyboardState _oldKS;

        public void Initialize()
        {
            var hatTexture = Scene.GameCore.Content.Load<Texture2D>("SceneGraph/hat");
            var staffTexture = Scene.GameCore.Content.Load<Texture2D>("SceneGraph/staff");

            var hatEntity = Scene.CreateEntity("hat")
                .AddComponent<EquipmentComponent>(0f, 90f)
                .AddComponent(new SpriteComponent(hatTexture));

            var staffEntity = Scene.CreateEntity("staff")
                .AddComponent<EquipmentComponent>(-80f, 10f)
                .AddComponent(new SpriteComponent(staffTexture));

            var characterEntity = Scene.GetEntity("character");
            characterEntity.AddChild(hatEntity);
            characterEntity.AddChild(staffEntity);
        }

        public void Update()
        {
            var ks = Keyboard.GetState();
            var entities = Scene.GetEntities(_ => MatchComponents(_));

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];

                var equipmentComponent = entity.GetComponent<EquipmentComponent>();
                var newPosition = entity.Parent.Transform.Position - equipmentComponent.OffsetPosition;

                entity.SetPosition(newPosition);

                if (entity.UniqueId == "hat" && ks.IsKeyDown(Keys.D1) && _oldKS.IsKeyUp(Keys.D1))
                    entity.SetActive(!entity.Active);

                if (entity.UniqueId == "staff" && ks.IsKeyDown(Keys.D2) && _oldKS.IsKeyUp(Keys.D2))
                    entity.SetActive(!entity.Active);
            }

            _oldKS = ks;
        }
    }
}
