using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;

namespace Curupira2D.Testbed.Systems.Physic
{
    class BorderControllerSystem : ECS.System, IInitializable
    {
        public void Initialize()
        {
            var horizontalBorderTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(Scene.ScreenWidth, 25, Color.LightCoral);
            var verticalBorderTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(25, Scene.ScreenHeight, Color.LightCoral);

            var horizontalBorderSpriteComponent = new SpriteComponent(horizontalBorderTexture);
            var verticalBorderSpriteComponent = new SpriteComponent(verticalBorderTexture);

            Scene.CreateEntity("left-border")
                .SetPosition(verticalBorderSpriteComponent.Origin.X, verticalBorderSpriteComponent.Origin.Y)
                .AddComponent(verticalBorderSpriteComponent)
                .AddComponent<BodyComponent>(verticalBorderTexture.Bounds.Size.ToVector2(), 1);

            Scene.CreateEntity("up-border")
                .SetPosition(horizontalBorderSpriteComponent.Origin.X, horizontalBorderSpriteComponent.Origin.Y)
                .AddComponent(horizontalBorderSpriteComponent)
                .AddComponent<BodyComponent>(horizontalBorderTexture.Bounds.Size.ToVector2(), 1);

            Scene.CreateEntity("right-border")
                .SetPosition(Scene.ScreenWidth - verticalBorderSpriteComponent.Origin.X, verticalBorderSpriteComponent.Origin.Y)
                .AddComponent(verticalBorderSpriteComponent)
                .AddComponent<BodyComponent>(verticalBorderTexture.Bounds.Size.ToVector2(), 1);

            Scene.CreateEntity("down-border")
                .SetPosition(horizontalBorderSpriteComponent.Origin.X, Scene.ScreenHeight - horizontalBorderSpriteComponent.Origin.Y)
                .AddComponent(horizontalBorderSpriteComponent)
                .AddComponent<BodyComponent>(horizontalBorderTexture.Bounds.Size.ToVector2(), 1);
        }
    }
}
