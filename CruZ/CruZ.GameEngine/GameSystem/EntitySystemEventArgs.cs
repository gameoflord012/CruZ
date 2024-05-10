using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem
{
    internal class SystemEventArgs : EventArgs
    {
        public readonly EntityCollection ActiveEntities;
        public readonly GameTime GameTime;

        public SystemEventArgs(IImmutableList<TransformEntity> activeEntities, GameTime gameTime)
        {
            ActiveEntities = new EntityCollection(activeEntities);
            GameTime = gameTime;
        }
    }
}
