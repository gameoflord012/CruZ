using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.StateMachine
{
    public class StateMachineComponent : Component
    {
        public void Add(StateBase state)
        {
            _states[state.GetType()] = state;
            state.OnAdded(this);
        }

        public void ChangeState(Type? ty)
        {
            if(ty == null)
                _currentState = null;
            else
                _currentState = GetState(ty);
        }

        private StateBase GetState(Type ty)
        {
            if(!_states.TryGetValue(ty, out StateBase? state))
                throw new ArgumentException("can't find state");
            return state;
        }

        internal void DoUpdate(GameTime gameTime)
        {
            _currentState?.DoUpdate(gameTime);
        }

        internal void DoDraw(GameTime gameTime)
        {
            _currentState?.DoDraw(gameTime);
        }

        Dictionary<Type, StateBase> _states = [];
        StateBase? _currentState;
    }
}
