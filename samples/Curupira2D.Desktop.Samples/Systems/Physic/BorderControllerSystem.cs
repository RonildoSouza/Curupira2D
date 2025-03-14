using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Components.Physics;
using Curupira2D.ECS.Systems;
using Curupira2D.Extensions;
using Microsoft.Xna.Framework;

namespace Curupira2D.Desktop.Samples.Systems.Physic
{
    class BorderControllerSystem : ECS.System, ILoadable
    {
        public void LoadContent()
        {
            var horizontalBorderTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(Scene.ScreenWidth, 25, Color.LightCoral);
            var verticalBorderTexture = Scene.GameCore.GraphicsDevice.CreateTextureRectangle(25, Scene.ScreenHeight, Color.LightCoral);

            var horizontalBorderSpriteComponent = new SpriteComponent(horizontalBorderTexture);
            var verticalBorderSpriteComponent = new SpriteComponent(verticalBorderTexture);

            Scene.CreateEntity("left-border", verticalBorderSpriteComponent.Origin.X, verticalBorderSpriteComponent.Origin.Y)
                .AddComponent(verticalBorderSpriteComponent, new BodyComponent(verticalBorderTexture.Bounds.Size.ToVector2(), EntityType.Static, EntityShape.Rectangle));

            Scene.CreateEntity("up-border", horizontalBorderSpriteComponent.Origin.X, horizontalBorderSpriteComponent.Origin.Y)
                .AddComponent(horizontalBorderSpriteComponent, new BodyComponent(horizontalBorderTexture.Bounds.Size.ToVector2(), EntityType.Static, EntityShape.Rectangle));

            Scene.CreateEntity("right-border", Scene.ScreenWidth - verticalBorderSpriteComponent.Origin.X, verticalBorderSpriteComponent.Origin.Y)
                .AddComponent(verticalBorderSpriteComponent, new BodyComponent(verticalBorderTexture.Bounds.Size.ToVector2(), EntityType.Static, EntityShape.Rectangle));

            Scene.CreateEntity("down-border", horizontalBorderSpriteComponent.Origin.X, Scene.ScreenHeight - horizontalBorderSpriteComponent.Origin.Y)
                .AddComponent(horizontalBorderSpriteComponent, new BodyComponent(horizontalBorderTexture.Bounds.Size.ToVector2(), EntityType.Static, EntityShape.Rectangle));
        }
    }
}
