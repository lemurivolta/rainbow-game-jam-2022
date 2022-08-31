using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SimpleStateMachine
{
    public class StateMachineRunner : MonoBehaviour
    {
        [Tooltip("The state machine to run")]
        public StateMachine StateMachine;

        [Tooltip("Name of the initial state")]
        public string InitialStateName;

        private string _CurrentStateName;

        private void OnEnable()
        {
            _CurrentStateName = InitialStateName;
            ForEachCurrentStateHooks(h => h.OnEnter(this));
        }

        /// <summary>
        /// Performs a transition. This could change the current state.
        /// </summary>
        /// <param name="transitionName">Name of the transition to perform</param>
        /// <returns>Whether the state has been changed.</returns>
        public bool PerformTransition(string transitionName)
        {
            foreach(var transition in StateMachine.Transitions)
            {
                if(transition.Name == transitionName &&
                    transition.From == _CurrentStateName)
                {
                    ForEachCurrentStateHooks(h => h.OnExit(this));
                    _CurrentStateName = transition.To;
                    ForEachCurrentStateHooks(h => h.OnEnter(this));
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get a state from its name.
        /// </summary>
        /// <param name="name">Name of the state to look for in the state machine.</param>
        /// <returns>The state.</returns>
        /// <exception cref="System.Exception">If no state could be found with given name.</exception>
        private StateMachine.State GetStateFromName(string name)
        {
            foreach (var state in StateMachine.States)
            {
                if (state.Name == name)
                {
                    return state;
                }
            }
            throw new System.Exception("Cannot find state " + name);
        }

        /// <summary>
        /// Call an action for every hook of the current state.
        /// Exceptions are logged as errors.
        /// </summary>
        /// <param name="action">The action to call on every hook of the current state.</param>
        private void ForEachCurrentStateHooks(System.Action<StateHooks> action)
        {
            foreach (var h in GetStateFromName(_CurrentStateName).StateHooks)
            {
                try
                {
                    action(h);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.Message + "\n" + e.StackTrace, this);
                }
            }
        }

        internal ConditionalWeakTable<System.Type, Component> BehavioursMap = new();
    }
}
