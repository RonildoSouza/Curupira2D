using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;

namespace Curupira2D.Desktop.Samples.Systems.TiledMap
{
    [RequiredComponent(typeof(IsometricGravityBallSystem), typeof(BodyComponent))]
    class IsometricGravityBallSystem : ECS.System, ILoadable
    {
        public void LoadContent()
        {
            var ballRadius = 12;
            var ballTexture = Scene.GameCore.GraphicsDevice.CreateTextureCircle(ballRadius, Color.Black * 0.8f);

            for (int i = 1; i <= 6; i++)
            {
                Scene.CreateEntity($"ball_{i}", default)
                    .AddComponent(
                        new SpriteComponent(ballTexture, drawInUICamera: false),
                        new BodyComponent(ballRadius, EntityType.Dynamic)
                        {
                            Restitution = 0.1f,
                            Friction = 0.8f,
                        });
            }
        }
    }
}
