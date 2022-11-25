using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.ECS
{
    internal sealed class EntityManager : IDisposable
    {
        readonly List<Entity> _entities = new List<Entity>();

        public Entity Create(string uniqueId, Vector2 position, string group = null, bool isCollidable = true)
        {
            if (_entities.Any(_ => _.UniqueId == uniqueId))
                throw new ArgumentException($"An entity with the same Id exists ({uniqueId})!");

            var entity = new Entity(uniqueId, position, group, isCollidable);
            _entities.Add(entity);

            return entity;
        }

        public Entity Create(string uniqueId, float x, float y, string group = null, bool isCollidable = true)
            => Create(uniqueId, new Vector2(x, y), group, isCollidable);

        public Entity Get(string uniqueId) => _entities.FirstOrDefault(_ => _.UniqueId == uniqueId);

        public IReadOnlyList<Entity> GetAll(Func<Entity, bool> match) => _entities.Where(match).ToList();

        public void Remove(Predicate<Entity> match) => _entities.RemoveAll(match);

        public void Remove(string uniqueId)
        {
            Remove(_ =>
            {
                if (_.UniqueId == uniqueId && _.Children.Any())
                {
                    foreach (var child in _.Children)
                        Remove(child.UniqueId);
                }

                return _.UniqueId == uniqueId;
            });
        }

        public void RemoveAll() => Remove(_ => true);

        public bool Exists(Func<Entity, bool> match) => _entities.Any(match);

        public void Dispose()
        {
            RemoveAll();

            GC.Collect();
        }
    }
}
