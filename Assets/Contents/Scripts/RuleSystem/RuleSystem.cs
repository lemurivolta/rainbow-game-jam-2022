using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RuleSystem : MonoBehaviour
{
    public delegate void StateChangeDelegate(bool newValue);

    [System.Serializable]
    public abstract class StateChangeNotifier
    {
        private List<StateChangeDelegate> Listeners = new();

        public void AddListener(StateChangeDelegate listener)
        {
            Listeners.Add(listener);
        }

        public abstract void AddApproachedByPlayerListener(UnityAction<CharacterInfo.Players> listener);

        public abstract void AddLeftByPlayerListener(UnityAction<CharacterInfo.Players> listener);

        public abstract void RemoveAllApproachedByPlayerListeners();

        public abstract void RemoveAllAddLeftByPlayerListeners();

        public void RemoveListener(StateChangeDelegate listener)
        {
            Listeners.Remove(listener);
        }

        public void RemoveAllListeners()
        {
            Listeners.Clear();
        }

        protected abstract bool CurrentState();

        protected void RaiseNewState(bool value)
        {
            foreach (var listener in Listeners)
            {
                listener(value);
            }
        }

        public virtual void OnEnable() { }

        public virtual void OnDisable() { }
    }

    [System.Serializable]
    public class StateCondition : StateChangeNotifier
    {
        public State State;

        public override void OnEnable()
        {
            State.Changed.AddListener(Listener);
        }

        public override void OnDisable()
        {
            State.Changed.RemoveListener(Listener);
        }

        private void Listener()
        {
            RaiseNewState(State.Value);
        }

        protected override bool CurrentState()
        {
            return State.Value;
        }

        public override void AddApproachedByPlayerListener(UnityAction<CharacterInfo.Players> listener)
        {
            if (State.ReferenceInteractable != null)
            {
                State.ReferenceInteractable.ApproachedByPlayer.AddListener(listener);
            }
        }

        public override void AddLeftByPlayerListener(UnityAction<CharacterInfo.Players> listener)
        {
            if (State.ReferenceInteractable != null)
            {
                State.ReferenceInteractable.LeftByPlayer.AddListener(listener);
            }
        }

        public override void RemoveAllApproachedByPlayerListeners()
        {
            if (State.ReferenceInteractable != null)
            {
                State.ReferenceInteractable.ApproachedByPlayer.RemoveAllListeners();
            }
        }

        public override void RemoveAllAddLeftByPlayerListeners()
        {
            if (State.ReferenceInteractable != null)
            {
                State.ReferenceInteractable.LeftByPlayer.RemoveAllListeners();
            }
        }
    }

    [System.Serializable]
    public class MultiStateCondition : StateChangeNotifier
    {
        public enum StatesKind
        {
            AllMustBeOn,
            AtLeastOneMustBeOn
        }

        public State[] States = System.Array.Empty<State>();

        public StatesKind Kind;

        private bool LastState;

        public override void OnEnable()
        {
            foreach (var state in States)
            {
                state.Changed.AddListener(Listener);
            }
            LastState = CurrentState();
        }

        public override void OnDisable()
        {
            foreach (var state in States)
            {
                state.Changed.RemoveListener(Listener);
            }
        }

        private void Listener()
        {
            var currentState = CurrentState();
            if (LastState != currentState)
            {
                LastState = currentState;
                RaiseNewState(currentState);
            }
        }

        protected override bool CurrentState()
        {
            var value = States[0].Value;
            for (var i = 1; i < States.Length; i++)
            {
                var value2 = States[i].Value;
                value = Kind == StatesKind.AllMustBeOn ? value && value2 : value || value2;
            }
            return value;
        }

        public override void AddApproachedByPlayerListener(UnityAction<CharacterInfo.Players> listener)
        {
            foreach (var state in States)
            {
                if (state.ReferenceInteractable != null)
                {
                    state.ReferenceInteractable.ApproachedByPlayer.AddListener(listener);
                }
            }
        }

        public override void AddLeftByPlayerListener(UnityAction<CharacterInfo.Players> listener)
        {
            foreach (var state in States)
            {
                if (state.ReferenceInteractable != null)
                {
                    state.ReferenceInteractable.LeftByPlayer.AddListener(listener);
                }
            }
        }

        public override void RemoveAllApproachedByPlayerListeners()
        {
            foreach (var state in States)
            {
                if (state.ReferenceInteractable != null)
                {
                    state.ReferenceInteractable.ApproachedByPlayer.RemoveAllListeners();
                }
            }
        }

        public override void RemoveAllAddLeftByPlayerListeners()
        {
            foreach (var state in States)
            {
                if (state.ReferenceInteractable != null)
                {
                    state.ReferenceInteractable.LeftByPlayer.RemoveAllListeners();
                }
            }
        }
    }

    [System.Serializable]
    public class Rule
    {
        [SerializeReference]
        public StateChangeNotifier State;
        public Actionable[] WhenTrue = System.Array.Empty<Actionable>();
        public Actionable[] WhenFalse = System.Array.Empty<Actionable>();
    }

    public Rule[] Rules = System.Array.Empty<Rule>();

    private void OnEnable()
    {
        foreach (var rule in Rules)
        {
            rule.State.AddListener((newValue) =>
            {
                if (newValue)
                {
                    foreach (var activity in rule.WhenTrue)
                    {
                        activity.PerformAction();
                    }
                }
                else if (!newValue)
                {
                    foreach (var activity in rule.WhenFalse)
                    {
                        activity.PerformAction();
                    }
                }
            });
            var allActionables = new HashSet<Actionable>(rule.WhenTrue);
            allActionables.UnionWith(rule.WhenFalse);
            rule.State.AddApproachedByPlayerListener(player =>
            {
                foreach (var actionable in allActionables)
                {
                    actionable.PlayerJustApproachedControl(player);
                }
            });
            rule.State.AddLeftByPlayerListener(player =>
            {
                foreach (var actionable in allActionables)
                {
                    actionable.PlayerJustLeftControl(player);
                }
            });
            rule.State.OnEnable();
        }
    }

    private void OnDisable()
    {
        foreach (var rule in Rules)
        {
            rule.State.RemoveAllListeners();
            rule.State.RemoveAllApproachedByPlayerListeners();
            rule.State.RemoveAllAddLeftByPlayerListeners();
            rule.State.OnDisable();
        }
    }
}
