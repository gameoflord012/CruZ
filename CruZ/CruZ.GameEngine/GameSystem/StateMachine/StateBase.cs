using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.StateMachine
{
    public class StateBase : IDisposable
    {
        internal void DoAdded(StateMachineComponent machine)
        {
            Machine = machine;
            OnAdded();
        }

        protected virtual void OnAdded()
        {

        }

        internal void DoUpdate(GameTime time)
        {
            OnUpdate(time);
        }

        internal void DoDraw(GameTime time)
        {
            OnDraw(time);
        }

        internal void DoStateEnter()
        {
            OnStateEnter();
        }

        internal void DoStateExit()
        {
            OnStateExit();
        }

        protected virtual void OnUpdate(GameTime gameTime)
        {
        
        }

        protected virtual void OnDraw(GameTime gameTime)
        {
            
        }

        protected virtual void OnStateEnter()
        {

        }

        protected virtual void OnStateExit()
        {

        }

        protected T GetData<T>(string dataKey)
        {
            return Machine.GetData<T>(dataKey);
        }

        protected void SetData(string dataKey, object data)
        {
            Machine.SetData(dataKey, data);
        }

        public virtual void Dispose()
        {
            
        }

        public StateMachineComponent Machine { get; private set; }
    }
}
