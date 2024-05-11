using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem
{
    public sealed class TransformEntity : IDisposable
    {
        public event Action<TransformEntity>? Destroying;
        public event Action<ComponentCollection>? ComponentsChanged;

        internal TransformEntity()
        {
            Id = s_entityCounter++;
            Name = $"New Entity({Id})";
            _transform = new();
            _components = new();
        }

        public T GetComponent<T>() where T : Component
        {
            return (T)GetComponent(typeof(T));
        }

        public bool TryGetComponent<T>([MaybeNullWhen(false)] out T component) where T : Component
        {
            if(_components.TryGetValue(typeof(T), out var value))
            {
                component = (T)value;
                return true;
            }

            component = default;
            return false;
        }

        public Component GetComponent(Type ty)
        {
            if(!HasComponent(ty))
                throw new ArgumentException($"Don't have component of type {ty}");

            return _components[ty];
        }

        public void AddComponent(Component component)
        {
            if(!_components.TryAdd(component.GetType(), component))
                throw new ArgumentException($"Component of type {component.GetType()} already added");

            component.InternalOnAttached(this);
            ComponentsChanged?.Invoke(new ComponentCollection(_components));
        }

        public bool HasComponent(Type ty)
        {
            return _components.ContainsKey(ty);
        }

        public IImmutableList<Component> GetAllComponents()
        {
            List<Component> comps = [];

            foreach(var comp in _components.Values)
                comps.Add(comp);

            return comps.ToImmutableList();
        }

        public void Destroy()
        {
            IsActive = false;
            ShouldDestroy = true;
            Destroying?.Invoke(this);
        }

        private void Parent_Destroying(TransformEntity parent)
        {
            Parent = null;
        }

        [ReadOnly(true)]
        public string Name
        {
            get;
            set;
        }

        public int Id
        {
            get;
            private set;
        }

        internal bool ShouldDestroy
        {
            get;
            private set;
        }

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        public TransformEntity? Parent
        {
            get => _parent;
            set
            {
                if(_parent == value) return;

                if(_parent != null)
                    _parent.Destroying -= Parent_Destroying;

                _parent = value;

                if(_parent != null)
                    _parent.Destroying += Parent_Destroying;
            }
        }

        public Transform Transform
        {
            get => _transform;
        }

        public Vector2 Position
        {
            get => Transform.Position;
            set => Transform.Position = value;
        }

        public Vector2 Scale
        {
            get => Transform.Scale;
            set => Transform.Scale = value;
        }

        internal IReadOnlyCollection<Component> Components
        {
            get => _components.Values;
        }

        private TransformEntity? _parent;
        private Dictionary<Type, Component> _components;
        private Transform _transform;
        private bool _isDisposed;
        private bool _isActive;

        public void Dispose()
        {
            if(_isDisposed) return;

            Destroying = default;
            ComponentsChanged = default;

            foreach(var component in GetAllComponents())
            {
                component.Dispose();
            }

            _isDisposed = true;
        }

        public override string ToString()
        {
            return Name;
        }

        private static int s_entityCounter = 0;
    }
}
