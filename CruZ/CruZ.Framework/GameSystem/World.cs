using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CruZ.Framework.GameSystem
{
    internal class World : IDisposable
    {
        public World() { }

        public World AddSystem(EntitySystem system)
        {
            _systems.Add(system);
            return this;
        }

        internal void AddEntity(TransformEntity e)
        {
            _entitiesToRemove.Remove(e);
            _entitiesToAdd.Add(e);
        }

        internal void RemoveEntity(TransformEntity e)
        {
            _entitiesToAdd.Remove(e);
            _entitiesToRemove.Add(e);
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
            ProcessEntitiesChanges();

            foreach (var system in _systems)
            {
                system.Update(new EntitySystemEventArgs(GetActiveEntities(), gameTime));
            }
        }

        public void SystemsDraw(GameTime gameTime)
        {
            ProcessEntitiesChanges();

            foreach (var system in _systems)
            {
                system.Draw(new EntitySystemEventArgs(GetActiveEntities(), gameTime));
            }
        }

        private List<TransformEntity> GetActiveEntities()
        {
            return _entities.Where(e => e.IsActive).ToList();
        }

        private void ProcessEntitiesChanges()
        {
            Trace.Assert(_entitiesToAdd.Intersect(_entitiesToRemove).Count() == 0);
            _entities.ExceptWith(_entitiesToRemove);
            _entities.UnionWith(_entitiesToAdd);
        }

        public void Dispose()
        {
            _systems.ForEach(e => e.Dispose());
            foreach (var e in Entities) e.Dispose();
        }

        public TransformEntity[] Entities { get => _entities.ToArray(); }

        List<EntitySystem> _systems = [];
        HashSet<TransformEntity> _entitiesToRemove = [];
        HashSet<TransformEntity> _entitiesToAdd = [];
        HashSet<TransformEntity> _entities = [];
    }
}
