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
        readonly IDictionary<string, int> _goldMinesAndAvailable = new Dictionary<string, int>();

        public void LoadContent()
        {
            _goldMineTexture = Scene.GameCore.Content.Load<Texture2D>("AI/GoldMineSpritesheet");

            for (int i = 0; i < 4; i++)
            {
                // Entity unique id and amount of gold available
                _goldMinesAndAvailable.Add($"goldMine{i}", 40);

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
                var entity = Scene.GetEntity(entityUniqueId);

                if (entity == null || !entity.Active)
                    continue;

                var spriteComponent = entity.GetComponent<SpriteComponent>();

                // State gold mine based on the amount of gold available
                if (_goldMinesAndAvailable[entityUniqueId] >= 30)
                {
                    spriteComponent.SourceRectangle = new Rectangle(0, 0, 28, 28);
                    continue;
                }

                if (_goldMinesAndAvailable[entityUniqueId] < 30 && _goldMinesAndAvailable[entityUniqueId] >= 20)
                {
                    spriteComponent.SourceRectangle = new Rectangle(28, 0, 28, 28);
                    continue;
                }

                if (_goldMinesAndAvailable[entityUniqueId] < 20 && _goldMinesAndAvailable[entityUniqueId] >= 10)
                {
                    spriteComponent.SourceRectangle = new Rectangle(56, 0, 28, 28);
                    continue;
                }

                if (_goldMinesAndAvailable[entityUniqueId] < 10 && _goldMinesAndAvailable[entityUniqueId] >= 0)
                {
                    spriteComponent.SourceRectangle = new Rectangle(84, 0, 28, 28);
                    continue;
                }
            }
        }

        public bool ThereIsGoldAvailable(string entityUniqueId)
            => _goldMinesAndAvailable.TryGetValue(entityUniqueId ?? string.Empty, out int available) && available > 0;

        public void DecreaseAvailableGold(string entityUniqueId)
        {
            if (_goldMinesAndAvailable.TryGetValue(entityUniqueId ?? string.Empty, out int available))
            {
                available--;
                if (available <= 0)
                {
                    _goldMinesAndAvailable.Remove(entityUniqueId);
                    Scene.RemoveEntity(entityUniqueId);
                    return;
                }

                _goldMinesAndAvailable[entityUniqueId] = available;
            }
        }
    }
}
