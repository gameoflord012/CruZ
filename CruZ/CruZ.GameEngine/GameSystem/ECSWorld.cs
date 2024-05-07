using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem
{
    internal class ECSWorld : IDisposable
    {
        public event Action<TransformEntity>? EntityAdded;
        public event Action<TransformEntity>? EntityRemoved;

        public ECSWorld() { }

        public ECSWorld AddSystem(EntitySystem system)
        {
            _systems.Add(system);
            return this;
        }

        internal void AddEntity(TransformEntity e)
        {
            _entities.Add(e);
            EntityAdded?.Invoke(e);
        }

        public void Initialize()
        {
            foreach (var system in _systems)
            {
                system.OnInitialize();
            }
        }

        public void SystemsUpdate(GameTime gameTime)
        {
            ProcessDirtyEntities();

            foreach (var system in _systems)
            {
                system.DoUpdate(new EntitySystemEventArgs(GetActiveEntities(), gameTime));
            }
        }

        public void SystemsDraw(GameTime gameTime)
        {
            ProcessDirtyEntities();

            foreach (var system in _systems)
            {
                system.DoDraw(new EntitySystemEventArgs(GetActiveEntities(), gameTime));
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
            foreach (var entity in removingEntities)
            {
                EntityRemoved?.Invoke(entity);
                entity.Dispose();
            }
        }

        public void Dispose()
        {
            _systems.ForEach(e => e.Dispose());
            
            foreach (var e in Entities)
            {
                _entities.Remove(e);
                e.Dispose();
            }
        }

        public IImmutableList<TransformEntity> Entities { get => _entities.ToImmutableList(); }

        HashSet<TransformEntity> _entities = [];

        List<EntitySystem> _systems = [];
    }
}
