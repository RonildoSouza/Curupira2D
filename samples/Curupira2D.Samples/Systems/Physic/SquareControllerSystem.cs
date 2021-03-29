using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;

namespace Curupira2D.Testbed.Systems.Physic
{
    class SquareControllerSystem : ECS.System, IInitializable
    {
        public void Initialize()
        {
            var squareTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(50, Color.Black * 0.6f);

            Scene.CreateEntity("square")
                .SetPosition(Scene.ScreenWidth * 0.6f, 100)
                .SetRotation(45)
                .AddComponent(new SpriteComponent(squareTexture))
                .AddComponent(new BodyComponent(squareTexture.Bounds.Size.ToVector2())
                {
                    EntityType = EntityType.Dynamic,
                    Restitution = 1f,
                });
        }
    }
}
