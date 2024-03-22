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
                system.DoUpdate(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            ProcessRemoveQueue();

            foreach (var system in _systems)
            {
                system.DoDraw(gameTime);
            }
        }

        private void ProcessRemoveQueue()
        {
            foreach (var toRemove in _removeQueue) _entities.Remove(toRemove);
            _removeQueue.Clear();
        }

        internal List<Entity> RemoveQueue { get => _removeQueue; }
        public Entity[] Entities { get => _entities.ToArray(); }

        HashSet<Entity> _entities = [];
        List<EntitySystem> _systems = [];
        List<Entity> _removeQueue = [];
        int _eCounter = 0;
    }
}
