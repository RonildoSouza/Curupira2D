using Curupira2D.ECS;
using Curupira2D.ECS.Components.Drawables;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.Desktop.Samples.Systems.BehaviorTreeAndPathfinder
{
    [RequiredComponent(typeof(GoldMineControllerSystem), typeof(SpriteComponent))]
    class GoldMineControllerSystem : ECS.System, ILoadable, IUpdatable
    {
        Texture2D _goldMineTexture;
        static readonly IDictionary<string, int> _goldMinesAndAvailable = new Dictionary<string, int>();

        public void LoadContent()
        {
            _goldMineTexture = Scene.GameCore.Content.Load<Texture2D>("AI/GoldMineSpritesheet");

            for (int i = 0; i < 4; i++)
            {
                _goldMinesAndAvailable.Add($"goldMine{i}", 100);

                Scene.CreateEntity(_goldMinesAndAvailable.Keys.ElementAt(i), default, "goldMines")
                    .AddComponent(new SpriteComponent(
                        texture: _goldMineTexture,
                        sourceRectangle: new Rectangle(0, 0, 28, 28),
                        layerDepth: 0.02f));
            }
        }

        public void Update()
        {
            for (int i = 0; i < _goldMinesAndAvailable.Count; i++)
            {
                var entityUniqueId = _goldMinesAndAvailable.Keys.ElementAt(i);

                if (_goldMinesAndAvailable[entityUniqueId] >= 100)
                    continue;

                var entity = Scene.GetEntity(entityUniqueId);

                if (entity == null || !entity.Active)
                    continue;

                var spriteComponent = entity.GetComponent<SpriteComponent>();

                if (_goldMinesAndAvailable[entityUniqueId] == 0)
                {
                    spriteComponent.SourceRectangle = new Rectangle(84, 0, 28, 28);
                    //_goldMinesAndAvailable[entityUniqueId] = -1;
                    continue;
                }

                if (_goldMinesAndAvailable[entityUniqueId] >= 75)
                {
                    spriteComponent.SourceRectangle = new Rectangle(28, 0, 28, 28);
                    continue;
                }

                if (_goldMinesAndAvailable[entityUniqueId] <= 50)
                {
                    spriteComponent.SourceRectangle = new Rectangle(56, 0, 28, 28);
                    continue;
                }

                if (_goldMinesAndAvailable[entityUniqueId] < 0)
                    entity.SetActive(false);
            }
        }

        public static bool ThereIsGoldAvailable(string entityUniqueId)
            => _goldMinesAndAvailable.TryGetValue(entityUniqueId, out int available) && available > 0;

        private int GetGoldMineState(string entityUniqueId)
        {
            if (_goldMinesAndAvailable.TryGetValue(entityUniqueId, out int available))
                return available;

            return 0;
        }
    }
}
