# MonoGame.Helper

```csharp
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;

namespace SpriteAnimation
{
    public class Game1 : GameCore
    {
        public Game1() : base() { }

        protected override void Initialize()
        {
            var scene = new Scene();

            var characterTexture = Content.Load<Texture2D>("character");

            var characterEntity = scene.CreateEntity("character");
            characterEntity.AddComponent(new SpriteComponent(characterTexture);

            SetScene(scene);
        }
    }
}
```