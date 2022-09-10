using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleStateMachine
{
    public class StateMachineRunner : MonoBehaviour
    {
        [Tooltip("The state machine to run")]
        public StateMachine StateMachine;

        [Tooltip("Name of the initial state")]
        public string InitialStateName;

        private string _CurrentStateName;

        public string CurrentStateName { get => _CurrentStateName; }

        /// <summary>
        /// Possible phases for a hook.
        /// </summary>
        public enum HookPhase
        {
            /// <summary>
            /// Enter happens on the initial state at state machine initialization,
            /// or at the end of a successful transition.
            /// </summary>
            Enter,
            /// <summary>
            /// Exit happens at the beginning of a successful transition.
            /// </summary>
            Exit
        }

        /// <summary>
        /// Description of a hook event.
        /// </summary>
        public class HookEventDescription
        {
            /// <summary>
            /// Phase this hook has been called on.
            /// </summary>
            public HookPhase Phase;
            /// <summary>
            /// Starting state of the transition.
            /// </summary>
            public string FromStateName;
            /// <summary>
            /// Final state of the transition.
            /// </summary>
            public string ToStateName;
        }

        /// <summary>
        /// A hook, used to connect events to the state machine.
        /// </summary>
        [System.Serializable]
        public class Hook {
            [Tooltip("Name of the state this hook is relative to.")]
            public string StateName;
            [Tooltip("Event called when this state is entered (initial state at initialization included).")]
            public UnityEvent<HookEventDescription> Enter;
            [Tooltip("Event called when this state is exited.")]
            public UnityEvent<HookEventDescription> Exit;
        }

        [Tooltip("All the hooks")]
        public Hook[] Hooks;

        private void Start()
        {
            _CurrentStateName = InitialStateName;
            var description = new HookEventDescription()
            {
                Phase = HookPhase.Enter,
                FromStateName = null,
                ToStateName = InitialStateName
            };
            ForEachHook(h => h.Enter.Invoke(description));
        }

        /// <summary>
        /// Performs a transition. This could change the current state.
        /// </summary>
        /// <param name="transitionName">Name of the transition to perform</param>
        /// <returns>Whether the state has been changed.</returns>
        public void PerformTransition(string transitionName)
        {
            foreach(var transition in StateMachine.Transitions)
            {
                if(transition.Name == transitionName &&
                    transition.From == _CurrentStateName)
                {
                    var description = new HookEventDescription()
                    {
                        Phase = HookPhase.Exit,
                        FromStateName = _CurrentStateName,
                        ToStateName = transition.To
                    };
                    ForEachHook(h => h.Exit.Invoke(description));
                    _CurrentStateName = description.ToStateName;
                    description.Phase = HookPhase.Enter;
                    ForEachHook(h => h.Enter.Invoke(description));
                    break;
                }
            }
        }

        /// <summary>
        /// Calls an action for every hook found for the current state
        /// </summary>
        /// <param name="action">The action to call.</param>
        public void ForEachHook(System.Action<Hook> action)
        {
            foreach(var hook in Hooks)
            {
                if(hook.StateName == _CurrentStateName)
                {
                    action(hook);
                }
            }
        }

        internal ConditionalWeakTable<System.Type, Component> BehavioursMap = new();
    }
}
