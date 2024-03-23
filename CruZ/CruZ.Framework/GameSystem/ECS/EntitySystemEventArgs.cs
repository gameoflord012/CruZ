using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CruZ.Framework.GameSystem.ECS
{
    internal class EntitySystemEventArgs : EventArgs
    {
        public readonly TransformEntity Entity;
        public readonly GameTime GameTime;

        public EntitySystemEventArgs(TransformEntity entity, GameTime gameTime)
        {
            Entity = entity;
            GameTime = gameTime;
        }
    }
}
