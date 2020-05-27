using Microsoft.Xna.Framework;
using MonoGame.Helper;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.Extensions;
using MonoGame.Helper.Physic.Components;
using MonoGame.Helper.Physic.Systems;

namespace Collision
{
    public class Game1 : GameCore
    {
        public Game1() : base() { }

        protected override void Initialize()
        {
            var scene = new Scene()
                .AddSystem<AetherPhysics2DSystem>();

            var groundHeight = 25;
            var ballTexture = GeometricPrimitives.CreateCircle(GraphicsDevice, 50, Color.Black);
            var squareTexture = GeometricPrimitives.CreateSquare(GraphicsDevice, 50, Color.Black);
            var groundTexture = GeometricPrimitives.CreateSquare(GraphicsDevice, GraphicsDevice.Viewport.Width, groundHeight, Color.Maroon);

            scene.CreateEntity("ball")
                .SetPosition(GraphicsDevice.Viewport.Width * 0.3f, 10)
                .AddComponent(new SpriteComponent(ballTexture))
                .AddComponent(new BodyComponent
                {
                    Radius = 50,
                    EntityType = EntityType.Dynamic,
                    EntityShape = EntityShape.Circle,
                    Torque = 100f,
                    LinearImpulse = new Vector2(0, 100),
                    Restitution = 0.6f,
                    Friction = 0.5f,
                });

            scene.CreateEntity("square")
                .SetPosition(GraphicsDevice.Viewport.Width * 0.6f, 10)
                .AddComponent(new SpriteComponent(squareTexture))
                .AddComponent(new BodyComponent
                {
                    EntityType = EntityType.Dynamic,
                    Torque = 100f,
                    LinearImpulse = new Vector2(0, 100),
                    Restitution = 0.6f,
                    Friction = 0.5f,
                });

            scene.CreateEntity("ground")
                .SetPosition(0, GraphicsDevice.Viewport.Height - groundHeight)
                .AddComponent(new SpriteComponent(groundTexture))
                .AddComponent<BodyComponent>();

            SetScene(scene);
        }
    }
}
