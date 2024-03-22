using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CruZ.Framework.GameSystem.ECS
{
    internal class World
    {
        public World() { }

        public World AddSystem(EntitySystem system)
        {
            _systems.Add(system);
            return this;
        }

        public void Update(GameTime gameTime)
        {
            ProcessRemoveQueue();

            foreach (var system in _systems)
            {
                system.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            ProcessRemoveQueue();

            foreach (var system in _systems)
            {
                system.Draw(gameTime);
            }
        }

        private void ProcessRemoveQueue()
        {
            foreach (var toRemove in _removeQueue) _entities.Remove(toRemove);
            _removeQueue.Clear();
        }

        internal List<TransformEntity> RemoveQueue { get => _removeQueue; }
        public TransformEntity[] Entities { get => _entities.ToArray(); }

        HashSet<TransformEntity> _entities = [];
        List<EntitySystem> _systems = [];
        List<TransformEntity> _removeQueue = [];
        int _eCounter = 0;
    }
}
