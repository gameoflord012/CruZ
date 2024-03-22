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

        public void Update(GameTime gameTime)
        {
            OnUpdate(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            OnDraw(gameTime);
        }

        protected virtual void OnUpdate(GameTime gameTime)
        {

        }

        protected virtual void OnDraw(GameTime gameTime)
        {

        }
    }
}
