using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleStateMachine
{
    [CreateAssetMenu(fileName = "StateMachine", menuName = "Simple State Machine/Create")]
    public class StateMachine : ScriptableObject
    {
        [System.Serializable]
        public struct Transition
        {
            public string Name;
            public string From;
            public string To;
        }

        public Transition[] Transitions;
    }
}
