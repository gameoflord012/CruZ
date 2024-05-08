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
        public StateData StateData => Machine.InjectedStateData;

        internal void AttachStateMachine(StateMachineComponent machine)
        {
            Machine = machine;
            OnStateMachineAttached();
        }

        protected virtual void OnStateMachineAttached()
        {

        }

        internal void DoUpdate(GameTime time)
        {
            OnStateUpdate(time);
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

        internal bool GetCanTransitionTo()
        {
            return CanTransitionTo();
        }

        protected virtual void OnStateUpdate(GameTime gameTime)
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

        protected virtual bool CanTransitionTo()
        {
            return true;
        }

        internal void DoTransitionChecking()
        {
            OnTransitionChecking();
        }

        protected virtual void OnTransitionChecking()
        {

        }

        protected void Check(Type ty)
        {
            Machine.SetNextState(ty, true);
        }

        public virtual void Dispose()
        {
            
        }

        public StateMachineComponent Machine { get; private set; } = null!;
    }

    public class StateBase<T> : StateBase where T : StateData
    {
        public new T StateData => base.StateData as T ?? throw new InvalidOperationException();
    }
}
