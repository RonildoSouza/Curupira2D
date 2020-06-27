# MonoGame.Helper 
![Create Release MonoGame.Helper](https://github.com/RonildoSouza/MonoGame.Helper/workflows/Create%20Release%20MonoGame.Helper/badge.svg)

## Simple Using
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
        }
    }
}
```
