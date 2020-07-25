using MonoGame.Helper.ECS;

namespace MonoGame.Helper.Physic
{
    public abstract class SystemPhysics : ECS.System
    {
        protected new ScenePhysics Scene { get; private set; }

        public override void SetScene(Scene scene)
        {
            base.SetScene(scene);
            Scene = (ScenePhysics)base.Scene;
        }
    }
}
