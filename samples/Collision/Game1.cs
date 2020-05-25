using Microsoft.Xna.Framework;
using MonoGame.Helper;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using MonoGame.Helper.ECS.Components.Physics;
using MonoGame.Helper.ECS.Systems.Physics;
using MonoGame.Helper.Extensions;

namespace Collision
{
    public class Game1 : GameCore
    {
        public Game1() : base() { }

        protected override void Initialize()
        {
            var scene = new Scene()
                .AddSystem<AetherPhysics2DSystem>();

            var ballRadius = 50;
            var groundHeight = 20;
            var ballTexture = GeometricPrimitives.CreateCircle(GraphicsDevice, ballRadius, Color.Black);
            var groundTexture = GeometricPrimitives.CreateSquare(GraphicsDevice, GraphicsDevice.Viewport.Width, groundHeight, Color.Maroon);

            scene.CreateEntity("ball")
                .SetPosition(GraphicsDevice.Viewport.Width / 2 - ballRadius, 10)
                .AddComponent(new SpriteComponent(ballTexture))
                .AddComponent(new BodyComponent
                {
                    Radius = 50,
                    EntityType = EntityType.Dynamic,
                    EntityShape = EntityShape.Circle,
                    Torque = 100f,
                    LinearImpulse = new Vector2(200, 100),
                    Restitution = 0.3f,
                    Friction = 0.5f,
                });

            scene.CreateEntity("ground")
                .SetPosition(0, GraphicsDevice.Viewport.Height - groundHeight)
                .AddComponent(new SpriteComponent(groundTexture))
                .AddComponent(new BodyComponent
                {
                    Restitution = 0.3f,
                    Friction = 0.5f,
                });

            SetScene(scene);
        }
    }
}
