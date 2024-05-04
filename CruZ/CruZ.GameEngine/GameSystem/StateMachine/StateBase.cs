using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.StateMachine
{
    public class StateBase
    {
        internal void OnAdded(StateMachineComponent machine)
        {
            Machine = machine;
        }

        internal void DoUpdate(GameTime time)
        {
            OnUpdate(time);
        }

        internal void DoDraw(GameTime time)
        {
            OnDraw(time);
        }

        protected virtual void OnUpdate(GameTime gameTime)
        {
        
        }

        protected virtual void OnDraw(GameTime gameTime)
        {
            
        }

        public StateMachineComponent Machine { get; private set; }
    }
}
