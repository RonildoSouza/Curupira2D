using Curupira2D.Samples.Common.Scenes;
using Curupira2D.Samples.Systems.SceneGraph;

namespace Curupira2D.Samples.Scenes
{
    class SceneGraphScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(SceneGraphScene));

            AddSystem<CharacterMovementSystem>();
            AddSystem<EquipmentMovimentSystem>();

            ShowControlTips("MOVIMENT: Keyboard Arrows\nEQUIPMENTS: Key 1, 2");

            base.LoadContent();
        }
    }
}
