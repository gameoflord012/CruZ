using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem
{
    internal class ECSWorld : IDisposable
    {
        public event Action<TransformEntity>? EntityAdded;
        public event Action<TransformEntity>? EntityRemoved;

        public ECSWorld()
        { }

        public T GetSystem<T>() where T : EntitySystem
        {
            foreach(var system in _systems)
            {
                if(system is T)
                {
                    return (T)system;
                }
            }

            throw new InvalidOperationException();
        }

        public ECSWorld AddSystem(EntitySystem system)
        {
            _systems.Add(system);
            system.AddedInternal(this);
            return this;
        }

        internal void AddEntity(TransformEntity e)
        {
            _entities.Add(e);
            EntityAdded?.Invoke(e);
        }

        internal void Initialize()
        {
            foreach(var system in _systems)
            {
                system.OnInitialize();
            }
        }

        internal void UpdateSystems(GameTime gameTime)
        {
            ProcessDestroyingEntity();

            foreach(var system in _systems)
            {
                system.InternalUpdate(new SystemEventArgs(GetActiveEntities(), gameTime));
            }
        }

        internal void DrawSystems(GameTime gameTime)
        {
            ProcessDestroyingEntity();

            foreach(var system in _systems)
            {
                system.InternalDraw(new SystemEventArgs(GetActiveEntities(), gameTime));
            }
        }

        private IEnumerable<TransformEntity> GetActiveEntities()
        {
            return _entities.Where(e => e.IsActive);
        }

        private void ProcessDestroyingEntity()
        {
            var removingEntities = _entities.Where(e => e.ShouldDestroy);
            _entities.ExceptWith(removingEntities);
            
            foreach(var entity in removingEntities)
            {
                EntityRemoved?.Invoke(entity);
                entity.Dispose();
            }
        }

        public IReadOnlyCollection<TransformEntity> Entities
        {
            get => _entities;
        }

        private HashSet<TransformEntity> _entities = [];
        private List<EntitySystem> _systems = [];

        public void Dispose()
        {
            foreach(var system in _systems)
            {
                system.Dispose();
            }

            foreach(var e in Entities)
            {
                e.Dispose();
            }
        }
    }
}
