using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Helper.ECS
{
    internal sealed class EntityManager : IDisposable
    {
        readonly List<Entity> _entities = new List<Entity>();
        static readonly Lazy<EntityManager> _entityManager = new Lazy<EntityManager>(() => new EntityManager());

        EntityManager() { }

        public static EntityManager Instance => _entityManager.Value;

        public Entity CreateEntity(string uniqueId)
        {
            if (_entities.Any(_ => _.UniqueId == uniqueId))
                throw new ArgumentException($"An entity with the same Id exists ({uniqueId})!");

            var entity = new Entity(uniqueId);
            _entities.Add(entity);

            return entity;
        }

        public Entity GetEntity(string uniqueId) => _entities.FirstOrDefault(_ => _.UniqueId == uniqueId);

        public IReadOnlyList<Entity> GetEntities(Func<Entity, bool> match) => _entities.Where(match).ToList();

        public void DestroyEntity(Predicate<Entity> match) => _entities.RemoveAll(match);

        public void DestroyEntity(string uniqueId) => DestroyEntity(_ => _.UniqueId == uniqueId);

        public void Dispose()
        {
            _entities.Clear();
            _entityManager.Value.Dispose();

            GC.Collect();
        }
    }
}
