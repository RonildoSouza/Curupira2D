using Microsoft.Xna.Framework;
using MonoGame.Helper.ECS;
using MonoGame.Helper.Physic.Systems;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGame.Helper.Physic
{
    public class ScenePhysics : Scene
    {
        readonly bool _debugActive;
        Vector2 _gravity;

        public ScenePhysics(Vector2 gravity = default, bool debugActive = false)
        {
            if (gravity == default)
                SetGravity(new Vector2(0f, 9.80665f));

            _debugActive = debugActive;
        }

        public World World { get; private set; }

        public ScenePhysics SetGravity(Vector2 gravity)
        {
            if (World == null)
                _gravity = gravity;
            else
                World.Gravity = gravity;

            return this;
        }

        public override void Initialize()
        {
            World = new World(_gravity);

            AddSystem<AetherPhysics2DSystem>();

            if (_debugActive)
                AddSystem<AetherPhysics2DDiagnosticsSystem>(default, default, default);

            base.Initialize();
        }

        public override void Dispose()
        {
            World.Clear();
            World = null;

            base.Dispose();
        }
    }
}
