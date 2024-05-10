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

        internal void InternalUpdate(StateUpdateArgs args)
        {
#pragma warning disable CS0612 // Type or member is obsolete
            OnStateUpdate(args.GameTime); // legacy purpose, should only call the last one
#pragma warning restore CS0612 // Type or member is obsolete
            OnStateUpdate(args);
        }

        internal void InternalDraw(GameTime time)
        {
            OnDraw(time);
        }

        internal void InternalStateEnter()
        {
            OnStateEnter();
        }

        internal void InternalStateExit()
        {
            OnStateExit();
        }

        internal bool GetCanTransitionTo()
        {
            return CanTransitionTo();
        }

        [Obsolete]
        protected virtual void OnStateUpdate(GameTime gameTime)
        {
        
        }

        protected virtual void OnStateUpdate(StateUpdateArgs args)
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

        internal void InternalTransitionChecking()
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
