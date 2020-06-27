using Microsoft.Xna.Framework.Graphics;
using MonoGame.Helper;
using MonoGame.Helper.ECS;
using MonoGame.Helper.ECS.Components.Drawables;
using SceneGraph.Components;
using SceneGraph.Systems;

namespace SceneGraph
{
    public class Game1 : GameCore
    {
        public Game1() : base() { }

        protected override void Initialize()
        {
            var scene = new Scene()
                .AddSystem<CharacterMovementSystem>()
                .AddSystem<EquipmentMovimentSystem>();

            var characterTexture = Content.Load<Texture2D>("character");
            var hatTexture = Content.Load<Texture2D>("hat");
            var staffTexture = Content.Load<Texture2D>("staff");

            var characterEntity = scene.CreateEntity("character")
                                       .SetPosition(400, 240)
                                       .AddComponent(new SpriteComponent(characterTexture));

            var hatEntity = scene.CreateEntity("hat")
                                 .AddComponent<EquipmentComponent>(0f, 90f)
                                 .AddComponent(new SpriteComponent(hatTexture));

            var staffEntity = scene.CreateEntity("staff")
                                 .AddComponent<EquipmentComponent>(-80f, 10f)
                                 .AddComponent(new SpriteComponent(staffTexture));

            characterEntity.AddChild(hatEntity);
            characterEntity.AddChild(staffEntity);

            SetScene(scene);
        }
    }
}
