using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CruZ.Framework.GameSystem.ECS
{
    internal class EntitySystem
    {
        public virtual void Initialize() { }

        public void DoUpdate(GameTime gameTime)
        {
            Update(gameTime);
        }

        public void DoDraw(GameTime gameTime)
        {
            Draw(gameTime);
        }

        protected virtual void Update(GameTime gameTime)
        {

        }

        protected virtual void Draw(GameTime gameTime)
        {

        }
    }
}
