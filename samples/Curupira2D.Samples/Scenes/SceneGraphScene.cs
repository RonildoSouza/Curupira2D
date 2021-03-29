using Curupira2D.Testbed.Common.Scenes;
using Curupira2D.Testbed.Systems.SceneGraph;

namespace Curupira2D.Testbed.Scenes
{
    class SceneGraphScene : SceneBase
    {
        public override void Initialize()
        {
            SetTitle(nameof(SceneGraphScene));

            AddSystem<CharacterMovementSystem>();
            AddSystem<EquipmentMovimentSystem>();

            ShowControlTips(120, 50, "MOVIMENT: Keyboard Arrows\nEQUIPMENTS: Key 1, 2");

            base.Initialize();
        }
    }
}
