using System;
using System.Collections.Generic;

using CruZ.GameEngine.Utility;

using Microsoft.Xna.Framework;

namespace CruZ.GameEngine.GameSystem.StateMachine
{
    public class StateMachineComponent : Component
    {
        public void Add(StateBase state)
        {
            _states[state.GetType()] = state;
            state.DoAdded(this);
        }

        public void SetNextState(Type? ty)
        {
            if(ty == null)
            {
                _currentState?.DoStateExit();
                _currentState = null;
            }
            else
            {
                
                var nextState = GetState(ty);
                if(_currentState == nextState) return;
                _nextState = nextState;
            }
        }

        public T GetData<T>(string dataKey)
        {
            if(!_data.TryGetValue(dataKey, out object? value))
                throw new ArgumentException("dataKey");
            return (T)value;
        }

        public void SetData(string dataKey, object data)
        {
            _data[dataKey] = data;
        }

        private StateBase GetState(Type ty)
        {
            if(!_states.TryGetValue(ty, out StateBase? state))
                throw new ArgumentException("can't find state");
            return state;
        }

        internal void DoUpdate(GameTime gameTime)
        {
            CheckTransition();

            SetData("TotalGameTime", gameTime.TotalGameTime());
            _currentState?.DoUpdate(gameTime);
        }

        internal void DoDraw(GameTime gameTime)
        {
            CheckTransition();

            SetData("TotalGameTime", gameTime.TotalGameTime());
            _currentState?.DoDraw(gameTime);
        }

        private void CheckTransition()
        {
            if(_nextState == null) return;
            _currentState?.DoStateExit();
            _currentState = _nextState;
            _currentState.DoStateEnter();
            _nextState = null;
        }

        Dictionary<Type, StateBase> _states = [];
        StateBase? _currentState;
        StateBase? _nextState;

        Dictionary<string, object> _data = [];

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
