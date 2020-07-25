using Microsoft.Xna.Framework;
using MonoGame.Helper.Attributes;
using MonoGame.Helper.ECS.Components;
using MonoGame.Helper.ECS.Systems;
using tainicom.Aether.Physics2D.Diagnostics;

namespace MonoGame.Helper.Physic.Systems
{
    [RequiredComponent(typeof(DebugViewComponent))]
    internal class AetherPhysics2DDiagnosticsSystem : SystemPhysics, IInitializable, IUpdatable, IRenderable
    {
        DebugView _debugView;
        readonly Color _defaultShapeColor;
        readonly Color _sleepingShapeColor;
        readonly Color _textColor;

        public AetherPhysics2DDiagnosticsSystem(Color defaultShapeColor = default, Color sleepingShapeColor = default, Color textColor = default)
        {
            _defaultShapeColor = defaultShapeColor == default ? Color.Orange : defaultShapeColor;
            _sleepingShapeColor = sleepingShapeColor == default ? Color.DodgerBlue : sleepingShapeColor;
            _textColor = textColor == default ? Color.Black : textColor;
        }

        public void Initialize()
        {
            _debugView = new DebugView(Scene.World);
            _debugView.AppendFlags(DebugViewFlags.Shape);
            _debugView.AppendFlags(DebugViewFlags.Joint);
            _debugView.AppendFlags(DebugViewFlags.PerformanceGraph);
            _debugView.AppendFlags(DebugViewFlags.DebugPanel);
            _debugView.DefaultShapeColor = _defaultShapeColor;
            _debugView.SleepingShapeColor = _sleepingShapeColor;
            _debugView.TextColor = _textColor;

            _debugView.LoadContent(Scene.GameCore.GraphicsDevice, Scene.GameCore.Content);
        }

        public void Update()
        {
            _debugView.UpdatePerformanceGraph(Scene.World.UpdateTime);
        }

        public void Draw()
        {
            var matrix = Matrix.CreateOrthographicOffCenter(
                0f,
                Scene.GameCore.GraphicsDevice.Viewport.Width,
                Scene.GameCore.GraphicsDevice.Viewport.Height,
                0f, 0f, -1f);

            _debugView.RenderDebugData(matrix, Matrix.Identity, null, null, null, null, 1f);
        }
    }

    internal class DebugViewComponent : IComponent { }
}
