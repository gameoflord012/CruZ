using System;
using System.Collections.Generic;
using System.Linq;

using CruZ.Common.ECS;

namespace CruZ.Framework.GameSystem.ECS
{
    public class Entity : IDisposable
    {
        internal Entity(World world) 
        { 
            _world = world;
            Name = "Entity";
            Id = _entityCounter++;
        }

        public T GetComponent<T>() where T : Component
        {
            throw new NotImplementedException();
            //return (T)GetComponent(typeof(T));
        }

        public void TryGetComponent<T>(ref T? com) where T : Component
        {
            if (HasComponent(typeof(T))) com = GetComponent<T>();
        }

        public Component GetComponent(Type ty)
        {
            throw new NotImplementedException();
            //if (!ty.IsAssignableTo(typeof(Component)))
            //{
            //    throw new ArgumentException($"Type {ty} is not a Component Type");
            //}

            //var compTy = ComponentHelper.GetComponentType(ty);

            //if (!_tyToComp.ContainsKey(compTy))
            //{
            //    throw new($"Entity doesn't contain component of type {ty}");
            //}

            //return _tyToComp[compTy];
        }

        public void AddComponent(Component component)
        {
            //if (HasComponent(component.GetType()))
            //    throw new(string.Format("Component {0} already added", component));

            //_entity.Attach(component, component.ComponentType);
            //_comToEntity[component] = this;
            //_tyToComp[component.ComponentType] = component;

            //component.InternalOnAttached(this);
            //ComponentChanged?.Invoke(new ComponentCollection(_tyToComp));
        }

        public void RemoveComponent(Type compTy)
        {
            //var comp = GetComponent(compTy);

            //_entity.Detach(comp.ComponentType);
            //_comToEntity.Remove(comp);
            //_tyToComp.Remove(comp.ComponentType);

            //comp.InternalOnDetached(this);
            //ComponentChanged?.Invoke(new ComponentCollection(_tyToComp));
        }

        public bool HasComponent(Type ty)
        {
            throw new NotImplementedException();
            //if (ty.IsInterface)
            //{
            //    throw new ArgumentException($"Can't pass component type as interface {ty}");
            //}

            //var compTy = ComponentHelper.GetComponentType(ty);

            //return
            //    _entity.Has(compTy);
        }

        public static Component[] GetAllComponents(TransformEntity e)
        {
            throw new NotImplementedException();
            //return e._tyToComp.Values.ToArray();
        }

        public override string ToString()
        {
            return $"{Name}: {Id}";
        }

        public void Dispose()
        {
            _world.RemoveQueue.Add(this);
        }

        public int Id { get; private set; }
        public string Name { get; set; }

        Dictionary<Type, HashSet<Component>> components = [];
        World _world;

        static int _entityCounter = 0;
    }
}
