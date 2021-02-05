using Microsoft.Xna.Framework;
using MonoGame.Helper.ECS.Components;

namespace MonoGame.Helper.Samples.Components.SceneGraph
{
    class EquipmentComponent : IComponent
    {
        public EquipmentComponent(float offsetPosX, float offsetPosY)
        {
            OffsetPosition = new Vector2(offsetPosX, offsetPosY);
        }

        public Vector2 OffsetPosition { get; }
    }
}
