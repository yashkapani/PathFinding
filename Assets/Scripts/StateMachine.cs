using System.Collections.Generic;
using UnityEngine;

namespace AISandbox {
    public class StateMachine {

        public abstract class State {

            public State nextState;
            public bool transition;
            public string name;
            public abstract string Name { get; set; }
            public abstract void Enter();
            public abstract void Update();
            public abstract void Exit();
        }

        State _activeState;
        List<State> _states;

        public bool processed;

        public StateMachine()
        {
            _states = new List<State>();
        }

        public void AddState(State state) {
            _states.Add(state);
        }

        public void RemoveState( State state ) {
            _states.Remove(state);
        }

        public void RemoveState( string name ) {
            foreach (State state in _states)
                if (state.Name == name)
                    RemoveState(state);
        }

        public void SetActiveState( State state ) {
            if (_activeState != state)
            {
                if (_activeState != null)
                    _activeState.Exit();
                _activeState = state;
                _activeState.Enter();
            }
        }

        public void SetActiveState( string name ) {
            foreach (State state in _states)
                if (state.Name == name)
                    SetActiveState(state);
        }

        public void Update() {
            if(!_activeState.transition)
                _activeState.Update();
            else
            {
                if (_activeState.nextState != null)
                    SetActiveState(_activeState.nextState);
                else
                {
                    processed = true;
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        ActorStateMachine.gameover = false;
                        Application.LoadLevel(Application.loadedLevel);
                    }
                }
            }
        }
    }
}