using CruZ.Systems;
using CruZ.Utility;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace CruZ.Components
{
    public partial class TransformEntity : IEquatable<TransformEntity>
    {
        public event EventHandler<bool>         OnActiveStateChanged;
        public event EventHandler               RemoveFromWorldEvent;
        public event EventHandler<IComponent>   OnComponentAdded;

        public string           Name        = "";
        [Browsable(false)]
        public Entity           Entity      { get => _entity; }
        public TransformEntity? Parent      { get => _parent;       set => _parent = value; }
        public bool             IsActive    { get => _isActive;     set => SetIsActive(value); }
        public Transform        Transform   { get => _transform;    set => _transform = value; }
        public Vector3          Position    { get => Transform.Position; set => Transform.Position = value; }
        public Vector3          Scale       { get => Transform.Scale; set => Transform.Scale = value; }

        [Browsable(false)]
        public IComponent[]     Components  => GetAllComponents(this);

        public TransformEntity(Entity e)
        {
            _entity = e;
            _idToTransformEntity[_entity.Id] = this;
        }

        public T GetComponent<T>()
        {
            return (T)GetComponent(typeof(T));
        }

        public void TryGetComponent<T>(ref T? com) where T : IComponent
        {
            if(HasComponent(typeof(T))) 
                com = GetComponent<T>();
        }

        public object GetComponent(Type ty)
        {
            if (!ComponentManager.IsComponent(ty))
                throw new(string.Format("Type {0} is not component type", ty));

            IComponent com = CreateInstanceFrom(ty);

            if (!_addedComponents.ContainsKey(com.ComponentType))
            {
                throw new(string.Format("Entity doesn't contain {0}", ty));
            }

            return _addedComponents[com.ComponentType];
        }

        public void AddComponent(IComponent component)
        {
            if (HasComponent(component.ComponentType))
                throw new(string.Format("Component {0} already added", component));

            _entity.Attach(component, component.ComponentType);

            _comToEntity[component] = this;
            _addedComponents[component.ComponentType] = component;

            ProcessCallback(component);
        }

        public bool HasComponent(Type ty)
        {
            return _entity.Has(CreateInstanceFrom(ty).ComponentType);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? $"Entity({Entity.Id})" : Name;
        }

        private void ProcessCallback(IComponent component)
        {
            if (component is IComponentCallback)
            {
                var callback = (IComponentCallback)component;
                callback.OnAttached(this);
            }

            OnComponentAdded?.Invoke(this, component);
        }

        private void SetIsActive(bool value)
        {
            _isActive = value;
            OnActiveStateChanged?.Invoke(this, value);
        }
#pragma warning disable CS8767 
        public bool Equals(TransformEntity other)
        {
            if (_entity == null || other._entity == null) return _entity == other._entity;
            return other._entity.Id == _entity.Id;
        }
#pragma warning restore CS8767 

        Entity                          _entity;
        TransformEntity?                _parent;
        bool                            _isActive = false;
        Transform                       _transform = new();
        Dictionary<Type, IComponent>    _addedComponents = new();

        private static IComponent CreateInstanceFrom(Type ty)
        {
            return (IComponent)Helper.GetUnitializeObject(ty);
        }

        public static TransformEntity GetEntity(object component)
        {
            if (!_comToEntity.ContainsKey(component))
            {
                throw new("Im tired of this shit");
            }
            else
            {
                return _comToEntity[component];
            }
        }

        public static IComponent[] GetAllComponents(TransformEntity e)
        {
            return e._addedComponents.Values.ToArray();
        }

        public static TransformEntity GetTransformEntity(int eId)
        {
            Trace.Assert(_idToTransformEntity.ContainsKey(eId));
            return _idToTransformEntity[eId];
        }

        private static Dictionary<int, TransformEntity>     _idToTransformEntity = new();
        private static Dictionary<object, TransformEntity>  _comToEntity = new();

    }
}
