# MonoGame.Helper

Simple helper library to development 2D games with MonoGame using ECS (_Entity-Component-System_) game architecture.

![Create Release MonoGame.Helper](https://github.com/RonildoSouza/MonoGame.Helper/workflows/Create%20Release%20MonoGame.Helper/badge.svg)

## Basic Using
```csharp
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;

namespace Sample
{
    public class Game1 : GameCore
    {
        public Game1() : base() { }

        protected override void Initialize()
        {
            var scene = new Scene();

            var characterTexture = Content.Load<Texture2D>("character");

            var characterEntity = scene.CreateEntity("character");
            characterEntity.AddComponent(new SpriteComponent(characterTexture));

            SetScene(scene);

            base.Initialize();
        }
    }
}
```

## Third Party

| Name                                           | Link                                         |
|------------------------------------------------|----------------------------------------------|
| MonoGame                                       | https://github.com/MonoGame/MonoGame         |
| Aether.Physics2D, Aether.Physics2D.Diagnostics | https://github.com/tainicom/Aether.Physics2D |
