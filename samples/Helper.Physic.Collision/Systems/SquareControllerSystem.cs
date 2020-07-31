using Microsoft.Xna.Framework;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Components.Physics;
using MonoGame.Helper.ECS.Systems;
using MonoGame.Helper.Extensions;

namespace Collision.Systems
{
    public class SquareControllerSystem : MonoGame.Helper.ECS.System, IInitializable
    {
        public void Initialize()
        {
            var squareTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(50, Color.Black * 0.6f);

            Scene.CreateEntity("square")
                .SetPosition(Scene.ScreenWidth * 0.6f, 100)
                .SetRotation(45)
                .AddComponent(new SpriteComponent(squareTexture))
                .AddComponent(new BodyComponent
                {
                    EntityType = EntityType.Dynamic,
                    Restitution = 1f,
                });
        }
    }
}
