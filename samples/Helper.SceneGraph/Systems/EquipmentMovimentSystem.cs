using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Systems;
using SceneGraph.Components;

namespace SceneGraph.Systems
{
    [RequiredComponent(typeof(EquipmentComponent))]
    public class EquipmentMovimentSystem : MonoGame.Helper.ECS.System, IUpdatable
    {
        public void Update()
        {
            SceneMatchEntitiesIteration(entity =>
            {
                var equipmentComponent = entity.GetComponent<EquipmentComponent>();
                var newPosition = entity.Parent.Transform.Position - equipmentComponent.OffsetPosition;

                entity.SetPosition(newPosition);
            });
        }
    }
}
