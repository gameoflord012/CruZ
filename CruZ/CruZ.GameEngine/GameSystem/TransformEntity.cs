using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

using CruZ.GameEngine.Serialization;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem
{
    public sealed class TransformEntity : IDisposable
    {
        public event Action<TransformEntity>? RemovedFromWorld;
        public event Action<ComponentCollection>? ComponentsChanged;

        internal TransformEntity()
        {
            Id = s_entityCounter++;
            Name = $"New Entity({Id})";
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
            if (!HasComponent(ty))
                throw new ArgumentException($"Don't have component of type {ty}");

            return _components[ty];
        }

        public void AddComponent(Component component)
        {
            if (!_components.TryAdd(component.GetType(), component))
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

            foreach (var comp in _components.Values)
                comps.Add(comp);

            return comps.ToImmutableList();
        }

        public void RemoveFromWorld()
        {
            IsActive = false;
            ShouldRemove = true;
            RemovedFromWorld?.Invoke(this);
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

        internal bool ShouldRemove
        {
            get;
            private set;
        }

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        bool _isActive = false;

        public TransformEntity? Parent
        {
            get => _parent;
            set
            {
                if (_parent == value) return;

                if (_parent != null)
                    _parent.RemovedFromWorld -= Parent_RemovedFromWorld;

                _parent = value;

                if (_parent != null)
                    _parent.RemovedFromWorld += Parent_RemovedFromWorld;
            }
        }

        TransformEntity? _parent;

        private void Parent_RemovedFromWorld(TransformEntity parent)
        {
            Parent = null;
        }

        public Transform Transform
        {
            get => _transform;
            set => _transform = value;
        }

        Transform _transform = new();

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

        internal IImmutableList<Component> Components { get => _components.Values.ToImmutableList(); }

        Dictionary<Type, Component> _components = [];

        public override string ToString()
        {
            return Name;
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            ShouldRemove = true;

            RemovedFromWorld = default;
            ComponentsChanged = default;

            foreach (var component in GetAllComponents())
            {
                component.Dispose();
            }
        }

        bool _isDisposed = false;

        static int s_entityCounter = 0;
    }
}
