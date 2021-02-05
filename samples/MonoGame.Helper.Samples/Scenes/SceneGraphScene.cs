using MonoGame.Helper.Common.Scenes;
using MonoGame.Helper.Samples.Systems.SceneGraph;

namespace MonoGame.Helper.Samples.Scenes
{
    class SceneGraphScene : SceneBase
    {
        public override void Initialize()
        {
            SetTitle("SceneGraphScene");

            AddSystem<CharacterMovementSystem>();
            AddSystem<EquipmentMovimentSystem>();

            ShowControlTips(120, 50, "MOVIMENT: Keyboard Arrows\nEQUIPMENTS: Key 1, 2");

            base.Initialize();
        }
    }
}
