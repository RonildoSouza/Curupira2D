# Curupira2D
[![Build and Publish](https://github.com/RonildoSouza/Curupira2D/actions/workflows/dotnet.yml/badge.svg)](https://github.com/RonildoSouza/Curupira2D/actions/workflows/dotnet.yml)

Simple helper library to development 2D games with [MonoGame](https://www.monogame.net/) using ECS (_Entity-Component-System_) game architecture.

## Basic Using
```csharp
using Microsoft.Xna.Framework.Graphics;
using Curupira2D;
using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;

namespace Sample
{
    public class Game1 : GameCore
    {
        public Game1() : base() { }

        protected override void LoadContent()
        {
            SetScene<MyScene>();
            base.LoadContent();
        }
    }

    public class MyScene : Scene
    {
        public override void LoadContent()
        {
            var characterTexture = GameCore.GraphicsDevice.CreateTextureCircle(10, Color.Red);

            CreateEntity("character", ScreenCenter)
                .AddComponent(new SpriteComponent(characterTexture));

            base.LoadContent();
        }
    }
}
```

## Third Party

| Name                                                 | Link                                         |
|------------------------------------------------------|----------------------------------------------|
| MonoGame                                             | https://github.com/MonoGame/MonoGame         |
| Aether.Physics2D.MG                                  | https://github.com/nkast/Aether.Physics2D    |
| TiledLib.Net                                         | https://github.com/Ragath/TiledLib.Net       |
