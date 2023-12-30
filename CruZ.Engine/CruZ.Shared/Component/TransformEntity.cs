﻿using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace CruZ.Components
{
    public partial class TransformEntity : IEquatable<TransformEntity>
    {
        public TransformEntity(Entity e)
        {
            _entity = e;
            _idToTransformEntity[_entity.Id] = this;
        }

        public void AddComponent<T>(T component) where T : class
        {
            AddComponent(component, typeof(T));
        }

        public object GetComponent(Type ty)
        {
            return _addedComponents[ty];
        }

        public T GetComponent<T>()
        {
            return (T)GetComponent(typeof(T));
        }

        public void AddComponent(object component, Type ty)
        {
            _entity.Attach(component, ty);

            _comToEntity[component] = this;
            _addedComponents.Add(ty, component);

            if (component is IComponentAddedCallback)
            {
                var callback = (IComponentAddedCallback)component;
                callback.OnComponentAdded(this);
            }
        }

        public bool HasComponent(Type ty)
        {
            return _entity.Has(ty);
        }

        public void RemoveFromWorld()
        {
            IsActive = false;
            ECS.World.DestroyEntity(_entity);
        }

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public bool Equals(TransformEntity other)
        {
            if (_entity == null || other._entity == null) return _entity == other._entity;
            return other._entity.Id == _entity.Id;
        }
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

        public Transform Transform  { get => _transform; set => _transform = value; }
        public Entity Entity        { get => _entity; }
        public bool IsActive        { get => _isActive; set => _isActive = value; }

        Entity _entity;
        bool _isActive = false;
        Transform _transform = new();
        Dictionary<Type, object> _addedComponents = new();

        private static Dictionary<int, TransformEntity> _idToTransformEntity = new();
        private static Dictionary<object, TransformEntity> _comToEntity = new();

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

        public static KeyValuePair<Type, object>[] GetAllComponents(TransformEntity e)
        {
            return e._addedComponents.ToArray();
        }

        public static TransformEntity GetTransformEntity(int eId)
        {
            Trace.Assert(_idToTransformEntity.ContainsKey(eId));
            return _idToTransformEntity[eId];
        }
    }
}
