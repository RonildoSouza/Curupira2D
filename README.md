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
            var scene = new Scene();

            var characterTexture = Content.Load<Texture2D>("character");

            var characterEntity = scene.CreateEntity("character");
            characterEntity.AddComponent(new SpriteComponent(characterTexture));

            SetScene(scene);

            base.LoadContent();
        }
    }
}
```

## Third Party

| Name                                                 | Link                                         |
|------------------------------------------------------|----------------------------------------------|
| MonoGame                                             | https://github.com/MonoGame/MonoGame         |
| Aether.Physics2D.MG                                  | https://github.com/tainicom/Aether.Physics2D |
| TiledLib.Net                                         | https://github.com/Ragath/TiledLib.Net       |
