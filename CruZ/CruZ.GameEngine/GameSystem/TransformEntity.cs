using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

using CruZ.GameEngine.Serialization;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem
{
    [JsonConverter(typeof(TransformEntityJsonConverter))]
    public sealed class TransformEntity : IDisposable
    {
        public event Action<TransformEntity>? BecameDrity;
        public event Action<ComponentCollection>? ComponentsChanged;

        internal TransformEntity()
        {
            Id = _entityCounter++;
            Name = $"New Entity({Id})";
        }

        public T GetComponent<T>() where T : Component
        {
            return (T)GetComponent(typeof(T));
        }

        public void TryGetComponent<T>(out T? com) where T : Component
        {
            if (HasComponent(typeof(T))) com = GetComponent<T>();
            else com = null;
        }

        public Component GetComponent(Type ty)
        {
            if (!HasComponent(ty))
                throw new ArgumentException($"Don't have component of type {ty}");

            return _components[ty];
        }

        public void AddComponent(Component component)
        {
            if (HasComponent(component.GetType()))
                throw new ArgumentException($"Component of type {component.GetType()} already added");

            _components[component.GetType()] = component;

            component.InternalOnAttached(this);
            ComponentsChanged?.Invoke(new ComponentCollection(_components));
        }

        public void RemoveComponent(Type ty)
        {
            if (!HasComponent(ty))
                throw new ArgumentException($"{ty} already removed");

            var comp = GetComponent(ty);
            _components.Remove(ty);

            comp.InternalOnDetached(this);
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

        public override string ToString()
        {
            return Name;
        }

        public void SetDirty()
        {
            IsActive = false;
            IsDirty = true;
            BecameDrity?.Invoke(this);
        }

        [ReadOnly(true)]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public int Id
        {
            get;
            private set;
        }

        internal bool IsDirty { get; private set; }

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
                if (_parent == value) return;

                if (_parent != null)
                    _parent.BecameDrity -= Parent_BecameDrity;

                _parent = value;

                if (_parent != null)
                    _parent.BecameDrity += Parent_BecameDrity;
            }
        }

        private void Parent_BecameDrity(TransformEntity parent)
        {
            Parent = null;
        }

        public Transform Transform
        {
            get => _transform;
            set => _transform = value;
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

        string _name = "";
        bool _isActive = false;
        TransformEntity? _parent;

        internal IImmutableList<Component> Components { get => _components.Values.ToImmutableList(); }

        Dictionary<Type, Component> _components = [];
        Transform _transform = new();

        static int _entityCounter = 0;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            IsDirty = true;

            BecameDrity = default;
            ComponentsChanged = default;

            foreach (var component in GetAllComponents())
            {
                component.Dispose();
            }
        }

        bool _isDisposed = false;
    }
}
