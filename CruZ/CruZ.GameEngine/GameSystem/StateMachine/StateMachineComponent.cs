using System;
using System.Collections.Generic;

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

        internal void Update(StateUpdateArgs args)
        {
            InjectedStateData.TotalGameTime = args.GameTime.TotalGameTime();

            CheckTransition();
            _currentState?.InternalUpdate(args);
        }

        internal void Draw(GameTime gameTime)
        {
            InjectedStateData.TotalGameTime = gameTime.TotalGameTime();

            CheckTransition();
            _currentState?.InternalDraw(gameTime);
        }

        private void CheckTransition()
        {
            _currentState?.InternalTransitionChecking();

            if(stateUpdateRequired)
            {
                _currentState?.InternalStateExit();
                //Debug.WriteLine((_currentState == null ? "<None>" : $"<{_currentState.GetType().Name}>") + " EXIT");
                _currentState = _nextState;
                _currentState?.InternalStateEnter();
                //Debug.WriteLine((_currentState == null ? "<None>" : $"<{_currentState.GetType().Name}>") + " ENTER");
            }
            stateUpdateRequired = false;
        }

        public Type? CurrentState
            => _currentState?.GetType();
        public StateData InjectedStateData
        { get; set; }

        private Dictionary<Type, StateBase> _states = [];
        private StateBase? _currentState;
        private StateBase? _nextState;
        private bool stateUpdateRequired = false;

        public override void Dispose()
        {
            base.Dispose();

            foreach(var state in _states.Values)
            {
                state.Dispose();
            }
        }
    }
}
