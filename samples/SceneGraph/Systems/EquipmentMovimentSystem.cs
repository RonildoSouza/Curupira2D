using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Systems;
using SceneGraph.Components;
using System.Linq;

namespace SceneGraph.Systems
{
    [RequiredComponent(typeof(EquipmentComponent))]
    public class EquipmentMovimentSystem : MonoGame.Helper.ECS.System, IUpdatable
    {
        public void Update()
        {
            var entities = Scene.GetEntities(_ => Matches(_));

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities.ElementAt(i);
                var equipmentComponent = entity.GetComponent<EquipmentComponent>();
                var newPosition = entity.Parent.Transform.Position - equipmentComponent.OffsetPosition;

                entity.SetPosition(newPosition);
            }
        }
    }
}
