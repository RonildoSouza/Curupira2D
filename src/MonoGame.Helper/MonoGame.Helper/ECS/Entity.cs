using Microsoft.Xna.Framework;
using MonoGame.Helper.ECS.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Helper.ECS
{
    public sealed class Entity
    {
        readonly Dictionary<Type, IComponent> _components = new Dictionary<Type, IComponent>();
        readonly List<Entity> _children = new List<Entity>();

        public Entity(string uniqueId)
        {
            UniqueId = uniqueId;
            Active = true;
            Transform = new Transform();
        }

        public string UniqueId { get; }
        public bool Active { get; private set; }
        public Transform Transform { get; }
        public Entity Parent { get; private set; }
        public IReadOnlyList<Entity> Children => _children;

        public Entity SetPosition(float x, float y)
        {
            Transform.SetPosition(x, y);
            return this;
        }

        public Entity SetPosition(Vector2 position) => SetPosition(position.X, position.Y);

        public Entity SetRotation(float rotationInDegrees)
        {
            Transform.SetRotation(rotationInDegrees);
            return this;
        }

        public Entity SetTransform(Vector2 position, float rotationInDegrees)
        {
            Transform.SetTransform(position, rotationInDegrees);
            return this;
        }

        public Entity SetActive(bool active)
        {
            Active = active;
            return this;
        }

        public Entity AddComponent(IComponent component)
        {
            if (component != null && !_components.ContainsKey(component.GetType()))
                _components.Add(component.GetType(), component);

            return this;
        }

        public Entity AddComponent<TComponent>(params object[] args) where TComponent : IComponent
        {
            var component = Activator.CreateInstance(typeof(TComponent), args);
            return AddComponent(component as IComponent);
        }

        public void RemoveComponent<TComponent>() where TComponent : IComponent
        {
            if (_components.ContainsKey(typeof(TComponent)))
                _components.Remove(typeof(TComponent));
        }

        public void UpdateComponent(IComponent component)
        {
            if (component == null || !_components.ContainsKey(component.GetType()))
                return;

            _components[component.GetType()] = component;
        }

        public TComponent GetComponent<TComponent>() where TComponent : IComponent
        {
            _components.TryGetValue(typeof(TComponent), out IComponent component);
            return (TComponent)component;
        }

        public bool HasComponent<TComponent>() where TComponent : IComponent
            => HasComponent(typeof(TComponent));

        public bool HasAllComponentTypes(IEnumerable<Type> componentTypes)
            => componentTypes.All(_ => HasComponent(_));

        public bool HasAnyComponentTypes(IEnumerable<Type> componentTypes)
            => componentTypes.Any(_ => HasComponent(_));

        private bool HasComponent(Type componentType) => _components.ContainsKey(componentType);

        public Entity AddChild(Entity child)
        {
            if (_children.Contains(child))
                return this;

            _children.Add(child);
            child.Parent = this;
            return this;
        }

        public Entity RemoveChild(Entity child)
        {
            if (!_children.Contains(child))
                return this;

            _children.Remove(child);
            child.Parent = null;
            return this;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity entity))
                return false;

            return entity.UniqueId == UniqueId;
        }

        public override int GetHashCode()
        {
            return -401120461 + EqualityComparer<string>.Default.GetHashCode(UniqueId);
        }
    }
}
