using Microsoft.Xna.Framework;
using MonoGame.Helper.ECS.Components;

namespace Helper.SceneGraph.Components
{
    public class EquipmentComponent : IComponent
    {
        public EquipmentComponent(float offsetPosX, float offsetPosY)
        {
            OffsetPosition = new Vector2(offsetPosX, offsetPosY);
        }

        public Vector2 OffsetPosition { get; }
    }
}
