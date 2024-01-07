using CruZ.Systems;
using CruZ.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
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
        public Transform        Transform   { get => _transform;    set => _transform = value; }
        public Entity           Entity      { get => _entity; }
        public TransformEntity? Parent      { get => _parent;       set => _parent = value; }
        public bool             IsActive    { get => _isActive;     set => _isActive = value; }
        public string           NameId      = "";

        public TransformEntity(Entity e)
        {
            _entity = e;
            _idToTransformEntity[_entity.Id] = this;
        }

        public T GetComponent<T>()
        {
            return (T)GetComponent(typeof(T));
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
                throw new Exception(string.Format("Component {0} already added", component));

            _entity.Attach(component, component.ComponentType);

            _comToEntity[component] = this;
            _addedComponents[component.ComponentType] = component;

            ProcessCallback(component);
        }

        //public void RequireComponent(Type ty)
        //{
        //    if (HasComponent(ty)) return;

        //    var comInstance = CreateInstanceFrom(ty);
        //    AddComponent(comInstance);
        //}

        public bool HasComponent(Type ty)
        {
            return _entity.Has(CreateInstanceFrom(ty).ComponentType);
        }

        public void RemoveFromWorld()
        {
            IsActive = false;
            ECS.World.DestroyEntity(_entity);
        }

        private void ProcessCallback(IComponent component)
        {
            if (component is IComponentCallback)
            {
                var callback = (IComponentCallback)component;
                callback.OnEntityChanged(this);
            }
        }

#pragma warning disable CS8767 
        public bool Equals(TransformEntity other)
        {
            if (_entity == null || other._entity == null) return _entity == other._entity;
            return other._entity.Id == _entity.Id;
        }
#pragma warning restore CS8767 

        Entity                      _entity;
        TransformEntity?            _parent;
        bool                        _isActive = false;
        Transform                   _transform = new();
        Dictionary<Type, object>    _addedComponents = new();

        private static IComponent CreateInstanceFrom(Type ty)
        {
            return (IComponent)Helper.GetUnitializeObject(ty);
        }

        public static TransformEntity GetEntity(object component)
        {
            if (!_comToEntity.ContainsKey(component))
            {
                throw new Exception("Im tired of this shit");
            }
            else
            {
                return _comToEntity[component];
            }
        }

        public static object[] GetAllComponents(TransformEntity e)
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
