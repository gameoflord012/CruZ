using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem
{
    internal class EntitySystemEventArgs : EventArgs
    {
        public readonly EntityCollection ActiveEntities;
        public readonly GameTime GameTime;

        public EntitySystemEventArgs(List<TransformEntity> activeEntities, GameTime gameTime)
        {
            ActiveEntities = new EntityCollection(activeEntities);
            GameTime = gameTime;
        }
    }
}
