using MonoGame.Helper.ECS;
using MonoGame.Helper.Samples.Systems.Physic;

namespace MonoGame.Helper.Samples.Scenes
{
    class PhysicScene : Scene
    {
        public override void Initialize()
        {
            AddSystem<BallControllerSystem>();
            AddSystem<SquareControllerSystem>();
            AddSystem<BorderControllerSystem>();

            base.Initialize();
        }
    }
}
