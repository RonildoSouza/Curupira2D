using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Systems;
using Helper.SceneGraph.Components;

namespace Helper.SceneGraph.Systems
{
    [RequiredComponent(typeof(EquipmentComponent))]
    public class EquipmentMovimentSystem : MonoGame.Helper.ECS.System, IInitializable, IUpdatable
    {
        KeyboardState _oldKS;

        public void Initialize()
        {
            var hatTexture = Scene.GameCore.Content.Load<Texture2D>("hat");
            var staffTexture = Scene.GameCore.Content.Load<Texture2D>("staff");

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
            var entities = Scene.GetEntities(_ => MatcheComponents(_));

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
