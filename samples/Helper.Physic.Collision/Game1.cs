using Microsoft.Xna.Framework;
using MonoGame.Helper;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.Extensions;
using MonoGame.Helper.Physic;
using MonoGame.Helper.Physic.Components;

namespace Collision
{
    public class Game1 : GameCore
    {
        public Game1() : base() { }

        protected override void Initialize()
        {
            var scenePhysics = new ScenePhysics(default, true);

            var ballRadius = 25;
            var ballTexture = GraphicsDevice.CreateTextureCircle(ballRadius, Color.Black * 0.6f);
            var squareTexture = GraphicsDevice.CreateTextureRectangle(50, Color.Black * 0.6f);
            var groundTexture = GraphicsDevice.CreateTextureRectangle(GraphicsDevice.Viewport.Width, 25, Color.Maroon);

            scenePhysics.CreateEntity("ball")
                .SetPosition(GraphicsDevice.Viewport.Width * 0.4f, 100)
                .AddComponent(new SpriteComponent(ballTexture))
                .AddComponent(new BodyComponent
                {
                    Radius = ballRadius,
                    EntityType = EntityType.Dynamic,
                    EntityShape = EntityShape.Circle,
                    LinearImpulse = new Vector2(0, 100),
                    Restitution = 0.6f,
                    Friction = 0.5f,
                });

            scenePhysics.CreateEntity("square")
                .SetPosition(GraphicsDevice.Viewport.Width * 0.6f, 100)
                .SetRotation(45)
                .AddComponent(new SpriteComponent(squareTexture))
                .AddComponent(new BodyComponent
                {
                    EntityType = EntityType.Dynamic,
                    LinearImpulse = new Vector2(0, 100),
                    Restitution = 0.6f,
                    Friction = 0.5f,
                });

            var groundSpriteComponent = new SpriteComponent(groundTexture);
            scenePhysics.CreateEntity("ground")
                .SetPosition(groundSpriteComponent.Origin.X, GraphicsDevice.Viewport.Height - groundSpriteComponent.Origin.Y)
                .AddComponent(groundSpriteComponent)
                .AddComponent<BodyComponent>();

            SetScene(scenePhysics);
        }
    }
}
