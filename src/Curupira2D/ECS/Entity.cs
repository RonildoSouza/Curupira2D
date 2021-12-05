using Curupira2D.ECS.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Curupira2D.ECS
{
    public sealed class Entity : IEquatable<Entity>
    {
        readonly Dictionary<Type, IComponent> _components = new Dictionary<Type, IComponent>();
        readonly List<Entity> _children = new List<Entity>();
        Vector2 _tempPosition;
        float _tempRotation;

        internal event EventHandler<EventArgs> OnChange;

        internal Entity(string uniqueId, Vector2 position, string group, bool isCollidable)
        {
            UniqueId = uniqueId;
            Position = position;
            Rotation = 0f;
            Active = true;
            Group = group;
            IsCollidable = isCollidable;
        }

        internal Entity(string uniqueId, float x, float y, string group, bool isCollidable)
            : this(uniqueId, new Vector2(x, y), group, isCollidable) { }

        public string UniqueId { get; }
        public Vector2 Position { get; private set; }
        public float Rotation { get; private set; }
        public bool Active { get; private set; }
        public Entity Parent { get; private set; }
        public IReadOnlyList<Entity> Children => _children;
        public string Group { get; set; }
        public bool IsCollidable { get; private set; }

        internal IReadOnlyDictionary<Type, IComponent> Components => _components;

        public void SetPosition(float x, float y)
        {
            var newPosition = new Vector2(x, y);

            if (Position != newPosition)
                _tempPosition = Position;

            if (_tempPosition == newPosition)
                return;

            Position = newPosition;
            OnChange?.Invoke(this, null);
        }

        public void SetPosition(Vector2 position) => SetPosition(position.X, position.Y);

        public void SetPositionX(float x) => SetPosition(x, Position.Y);

        public void SetPositionY(float y) => SetPosition(Position.X, y);

        public Entity SetRotation(float rotationInDegrees)
        {
            if (Rotation != rotationInDegrees)
                _tempRotation = Rotation;

            if (_tempRotation == rotationInDegrees)
                return this;

            Rotation = rotationInDegrees;
            OnChange?.Invoke(this, null);

            return this;
        }

        public Entity SetActive(bool active)
        {
            OnChange?.Invoke(this, null);
            Active = active;
            return this;
        }

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

        public bool Equals(Entity other) => other != null && other.UniqueId == UniqueId;

        #region Methods of managing components
        public Entity AddComponent(IComponent component)
        {
            if (_components.TryAdd(component.GetType(), component))
                OnChange?.Invoke(this, null);

            return this;
        }

        public Entity AddComponent<TComponent>(params object[] args) where TComponent : IComponent
        {
            var component = Activator.CreateInstance(typeof(TComponent), args);
            return AddComponent(component as IComponent);
        }

        public void RemoveComponent<TComponent>() where TComponent : IComponent
        {
            if (_components.Remove(typeof(TComponent)))
                OnChange?.Invoke(this, null);
        }

        public void UpdateComponent(IComponent component)
        {
            if (component == null || !_components.ContainsKey(component.GetType()))
                return;

            _components[component.GetType()] = component;
            OnChange?.Invoke(this, null);
        }

        public IComponent GetComponent(Func<KeyValuePair<Type, IComponent>, bool> predicate)
        {
            return _components.FirstOrDefault(predicate).Value;
        }

        public TComponent GetComponent<TComponent>() where TComponent : IComponent
        {
            _components.TryGetValue(typeof(TComponent), out IComponent component);
            return (TComponent)component;
        }

        public bool HasComponent(Func<KeyValuePair<Type, IComponent>, bool> predicate) => _components.Any(predicate);

        public bool HasComponent<TComponent>() where TComponent : IComponent => HasComponent(typeof(TComponent));

        public bool HasAllComponentTypes(IEnumerable<Type> componentTypes) => componentTypes.All(_ => HasComponent(_));

        public bool HasAnyComponentTypes(IEnumerable<Type> componentTypes) => componentTypes.Any(_ => HasComponent(_));
        #endregion

        bool HasComponent(Type componentType) => _components.ContainsKey(componentType);
    }
}
