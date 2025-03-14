using Curupira2D.Desktop.Samples.Common.Scenes;
using Curupira2D.Desktop.Samples.Systems.SceneGraph;

namespace Curupira2D.Desktop.Samples.Scenes
{
    class SceneGraphScene : SceneBase
    {
        public override void LoadContent()
        {
            SetTitle(nameof(SceneGraphScene));

            AddSystem<CharacterMovementSystem>();
            AddSystem<EquipmentMovimentSystem>();

            ShowControlTips("MOVIMENT: Keyboard Arrows OR WASD\nEQUIPMENTS: Key 1, 2");

            base.LoadContent();
        }
    }
}
