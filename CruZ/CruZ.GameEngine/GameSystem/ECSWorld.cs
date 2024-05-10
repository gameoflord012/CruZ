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

        public ECSWorld() { }

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
            ProcessDirtyEntities();

            foreach(var system in _systems)
            {
                system.InternalUpdate(new SystemEventArgs(GetActiveEntities(), gameTime));
            }
        }

        internal void DrawSystems(GameTime gameTime)
        {
            ProcessDirtyEntities();

            foreach(var system in _systems)
            {
                system.InternalDraw(new SystemEventArgs(GetActiveEntities(), gameTime));
            }
        }

        private IImmutableList<TransformEntity> GetActiveEntities()
        {
            return _entities.Where(e => e.IsActive).ToImmutableList();
        }

        private void ProcessDirtyEntities()
        {
            var removingEntities = _entities.Where(e => e.ShouldRemove);
            _entities.ExceptWith(removingEntities);
            //
            // fire events
            //
            foreach(var entity in removingEntities)
            {
                EntityRemoved?.Invoke(entity);
                entity.Dispose();
            }
        }

        public void Dispose()
        {
            _systems.ForEach(e => e.Dispose());

            foreach(var e in Entities)
            {
                _entities.Remove(e);
                e.Dispose();
            }
        }

        public IImmutableList<TransformEntity> Entities { get => _entities.ToImmutableList(); }
        private HashSet<TransformEntity> _entities = [];
        private List<EntitySystem> _systems = [];
    }
}
