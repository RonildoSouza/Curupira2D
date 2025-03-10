﻿using Curupira2D.Diagnostics;
using Curupira2D.ECS.Components;
using Curupira2D.ECS.Systems;
using Curupira2D.ECS.Systems.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Curupira2D.ECS
{
    public interface ISystem
    {
        Scene Scene { get; }
        bool MatchActiveEntitiesAndComponents(Entity entity);
        void OnRemoveFromScene();
    }

    public abstract class System : ISystem
    {
        readonly List<Type> _requiredComponents = [];

        public System()
        {
            SetupRequiredComponents();
        }

        public Scene Scene { get; private set; }

        public virtual void SetScene(Scene scene)
        {
            AssertRequiredComponents(_requiredComponents);
            Scene = scene;
        }

        protected void AddRequiredComponent<TComponent>() where TComponent : IComponent => AddRequiredComponent(new Type[] { typeof(TComponent) });

        protected bool MatchComponents(Entity entity)
        {
            AssertRequiredComponents(_requiredComponents);
            return entity.HasAllComponentTypes(_requiredComponents);
        }

        public bool MatchActiveEntitiesAndComponents(Entity entity) => entity.Active && MatchComponents(entity);

        public virtual void OnRemoveFromScene() { }

        protected virtual void SetupRequiredComponents()
        {
            var requiredComponentAttrs = GetType().GetTypeInfo().GetCustomAttributes<RequiredComponentAttribute>();

            for (int i = 0; i < requiredComponentAttrs.Count(); i++)
                AddRequiredComponent(requiredComponentAttrs.ElementAt(i).ComponentTypes);
        }

        void AddRequiredComponent(Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];

                if (_requiredComponents.Contains(type) || !type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IComponent)))
                    continue;

                _requiredComponents.Add(type);
            }
        }

        void AssertRequiredComponents(List<Type> requiredComponentTypes)
        {
            var implementOnlyILoadable = GetType().GetTypeInfo().ImplementedInterfaces.Count() == 2
                && GetType().GetTypeInfo().ImplementedInterfaces.Contains(typeof(ISystem))
                && GetType().GetTypeInfo().ImplementedInterfaces.Contains(typeof(ILoadable));

            if (!requiredComponentTypes.Any() && GetType().Name != nameof(DebugSystem) && !implementOnlyILoadable)
                throw new Exception($"You should add required component for the system {GetType().Name}");
        }
    }
}
