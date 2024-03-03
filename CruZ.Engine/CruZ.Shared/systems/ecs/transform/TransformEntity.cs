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
        public event EventHandler<bool> OnActiveStateChanged;
        public event EventHandler RemoveFromWorldEvent;
        public event Action<ComponentCollection> ComponentChanged;

        [ReadOnly(true)]
        public string           Name        { get => _name;         set => _name = value; }
        [Browsable(false)]
        public Entity           Entity      { get => _entity; }
        public TransformEntity? Parent      { get => _parent;       set => _parent = value; }
        public bool             IsActive    { get => _isActive;     set => SetIsActive(value); }
        public Transform        Transform   { get => _transform;    set => _transform = value; }
        [Browsable(false)]
        public Vector3          Position    { get => Transform.Position; set => Transform.Position = value; }
        [Browsable(false)]
        public Vector3          Scale       { get => Transform.Scale; set => Transform.Scale = value; }

        [Browsable(false)]
        public Component[]     Components  => GetAllComponents(this);

        public TransformEntity(Entity e)
        {
            _entity = e;
            _idToTransformEntity[_entity.Id] = this;
        }

        public T GetComponent<T>() where T : Component
        {
            return (T)GetComponent(typeof(T));
        }

        public void TryGetComponent<T>(ref T? com) where T : Component
        {
            if(HasComponent(typeof(T))) com = GetComponent<T>();
        }

        public Component GetComponent(Type ty)
        {
            if(!ty.IsAssignableTo(typeof(Component)))
            {
                throw new ArgumentException($"Type {ty} is not a Component Type");
            }

            var compTy = ComponentHelper.GetComponentType(ty);

            if (!_tyToComp.ContainsKey(compTy))
            {
                throw new($"Entity doesn't contain component of type {ty}");
            }

            return _tyToComp[compTy];
        }

        public void AddComponent(Component component)
        {
            if (HasComponent(component.ComponentType))
                throw new(string.Format("Component {0} already added", component));

            _entity.Attach(component, component.ComponentType);
            _comToEntity[component] = this;
            _tyToComp[component.ComponentType] = component;

            component.InternalOnAttached(this);
            ComponentChanged?.Invoke(new ComponentCollection(_tyToComp));
        }

        public void RemoveComponent(Type compTy)
        {
            var comp = GetComponent(compTy);
            
            _entity.Detach(comp.ComponentType);
            _comToEntity.Remove(comp);
            _tyToComp.Remove(comp.ComponentType);

            comp.InternalOnDetached(this);
            ComponentChanged?.Invoke(new ComponentCollection(_tyToComp));
        }

        public bool HasComponent(Type ty)
        {
            if(ty.IsInterface)
            {
                throw new ArgumentException($"Can't pass component type as interface {ty}");
            }

            return _entity.Has(ComponentHelper.GetComponentType(ty));
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? $"Entity({Entity.Id})" : Name;
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
        Dictionary<Type, Component>     _tyToComp = new();
        string                          _name = "";

        //private static Component CreateInstanceFrom(Type ty)
        //{
        //    return (Component)PropertyHelper.GetUnitializeObject(ty);
        //}

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

        public static Component[] GetAllComponents(TransformEntity e)
        {
            return e._tyToComp.Values.ToArray();
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
