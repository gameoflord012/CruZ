using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.StateMachine
{
    public class StateMachineComponent : Component
    {
        public StateMachineComponent()
        {
            InjectedStateData = new();
        }

        public void Add(StateBase state)
        {
            _states[state.GetType()] = state;
            state.AttachStateMachine(this);
        }

        public void SetNextState(Type? ty, bool checking)
        {
            if(ty == null)
            {
                _nextState = null;
            }
            else
            {
                
                var nextState = GetState(ty);
                if(checking && (_currentState == nextState || !nextState.GetCanTransitionTo())) return;
                _nextState = nextState;
            }

            stateUpdateRequired = true;
        }

        private StateBase GetState(Type ty)
        {
            if(!_states.TryGetValue(ty, out StateBase? state))
                throw new ArgumentException("can't find state");
            return state;
        }

        internal void DoUpdate(GameTime gameTime)
        {
            InjectedStateData.TotalGameTime = gameTime.TotalGameTime();

            CheckTransition();
            _currentState?.DoUpdate(gameTime);
        }

        internal void DoDraw(GameTime gameTime)
        {
            InjectedStateData.TotalGameTime = gameTime.TotalGameTime();

            CheckTransition();
            _currentState?.DoDraw(gameTime);
        }

        private void CheckTransition()
        {
            _currentState?.DoTransitionChecking();

            if (stateUpdateRequired)
            {
                _currentState?.DoStateExit();
                //Debug.WriteLine((_currentState == null ? "<None>" : $"<{_currentState.GetType().Name}>") + " EXIT");
                _currentState = _nextState;
                _currentState?.DoStateEnter();
                //Debug.WriteLine((_currentState == null ? "<None>" : $"<{_currentState.GetType().Name}>") + " ENTER");
            }
            stateUpdateRequired = false;
        }

        public Type? CurrentState => _currentState?.GetType();

        Dictionary<Type, StateBase> _states = [];

        StateBase? _currentState;
        StateBase? _nextState;

        bool stateUpdateRequired = false;

        public StateData InjectedStateData { get; set; }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var state in _states.Values)
            {
                state.Dispose();
            }
        }
    }
}
