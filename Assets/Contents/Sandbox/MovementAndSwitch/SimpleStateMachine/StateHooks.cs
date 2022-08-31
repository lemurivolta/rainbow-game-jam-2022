using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SimpleStateMachine
{
    public class StateHooks : ScriptableObject
    {
        public virtual void OnEnter(StateMachineRunner runner) { }

        public virtual void OnExit(StateMachineRunner runner) { }

        protected TComponent GetComponent<TComponent>(StateMachineRunner smr)
            where TComponent : Component
        {
            var type = typeof(TComponent);
            if (smr.BehavioursMap.TryGetValue(type, out var behaviour))
            {
                return (TComponent)behaviour;
            }
            else
            {
                var component = smr.GetComponent<TComponent>();
                smr.BehavioursMap.Add(type, component);
                return component;
            }
        }
    }
}
