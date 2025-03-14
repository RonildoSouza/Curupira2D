using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;

namespace Curupira2D.Desktop.Samples.Systems.Physic
{
    class SquareControllerSystem : ECS.System, ILoadable
    {
        public void LoadContent()
        {
            var squareTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(50, Color.Black * 0.6f);

            Scene.CreateEntity("square", Scene.ScreenWidth * 0.6f, 100f)
                .SetRotation(45)
                .AddComponent(
                    new SpriteComponent(squareTexture),
                    new BodyComponent(squareTexture.Bounds.Size.ToVector2(), EntityType.Dynamic, EntityShape.Rectangle)
                    {
                        Restitution = 1f,
                    });
        }
    }
}
