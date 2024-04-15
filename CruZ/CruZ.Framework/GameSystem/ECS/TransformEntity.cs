using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using CruZ.Framework.Serialization;
using Microsoft.Xna.Framework;

namespace CruZ.Framework.GameSystem.ECS
{
    [JsonConverter(typeof(TransformEntityJsonConverter))]
    public class TransformEntity : IDisposable
    {
        public event EventHandler? RemovedFromWorld;
        public event Action<ComponentCollection>? ComponentsChanged;

        internal TransformEntity(World world)
        {
            _world = world;
            Id = _entityCounter++;
            Name = $"New Entity({Id})";

            world.AddEntity(this);
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
            if(!HasComponent(ty))
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
            if(!HasComponent(ty))
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

        public Component[] GetAllComponents()
        {
            HashSet<Component> comps = [];

            foreach (var comp in _components.Values)
                comps.Add(comp);

            return comps.ToArray();
        }

        public override string ToString()
        {
            return Name;
        }

        public void RemoveFromWorld()
        {
            IsActive = false;
            _world.RemoveEntity(this);
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

        public bool IsActive 
        { 
            get => _isActive; 
            set => _isActive = value; 
        }

        public TransformEntity? Parent
        { 
            get => _parent; 
            set => _parent = value;
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
        World _world;

        static int _entityCounter = 0;
        public void Dispose()
        {
            RemoveFromWorld();

            foreach (var e in GetAllComponents())
            {
                e.Dispose();
            }
        }
    }
}
