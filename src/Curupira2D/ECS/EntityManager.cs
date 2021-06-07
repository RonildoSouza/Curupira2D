using System;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.ECS
{
    internal sealed class EntityManager : IDisposable
    {
        readonly List<Entity> _entities = new List<Entity>();
        static readonly Lazy<EntityManager> _entityManager = new Lazy<EntityManager>(() => new EntityManager());

        EntityManager() { }

        public static EntityManager Instance => _entityManager.Value;

        public Entity Create(string uniqueId, string group = null)
        {
            if (_entities.Any(_ => _.UniqueId == uniqueId))
                throw new ArgumentException($"An entity with the same Id exists ({uniqueId})!");

            var entity = new Entity(uniqueId, group);
            _entities.Add(entity);

            return entity;
        }

        public Entity Get(string uniqueId) => _entities.FirstOrDefault(_ => _.UniqueId == uniqueId);

        public IReadOnlyList<Entity> GetAll(Func<Entity, bool> match) => _entities.Where(match).ToList();

        public void Remove(Predicate<Entity> match) => _entities.RemoveAll(match);

        public void Remove(string uniqueId) => Remove(_ => _.UniqueId == uniqueId);

        public void RemoveAll() => Remove(_ => true);

        public bool Exists(Func<Entity, bool> match) => _entities.Any(match);

        public void Dispose()
        {
            _entities.Clear();
            _entityManager.Value.Dispose();

            GC.Collect();
        }
    }
}
