using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ControlledByPlayerState", menuName = "Simple State Machine/MAS/Create Controller by Player State Hooks")]
public class ControlledByPlayerState : StateHooks
{
    public override void OnEnter(StateMachineRunner runner)
    {
        Debug.Log("Controlled: enter", runner);
        // hook animations and movements to the input for the correct player
        var characterInputHandler = GetComponent<CharacterInputHandler>(runner);
        var characterAnimationsHandler = GetComponent<CharacterAnimationsHandler>(runner);
        var characterMovementHandler = GetComponent<CharacterMovementHandler>(runner);
        var movementEvent = GetMovementEvent(characterInputHandler, runner);
        movementEvent.AddListener(characterAnimationsHandler.OnMovement);
        movementEvent.AddListener(characterMovementHandler.OnMovement);
        // show that we're controlling the selector
        GetComponent<CharacterSelectorHandler>(runner).ShowSelector(true);
    }

    public override void OnExit(StateMachineRunner runner)
    {
        Debug.Log("Controlled: exit", runner);
        // remove all events from this player
        var scoutInputHandler = GetComponent<CharacterInputHandler>(runner);
        GetMovementEvent(scoutInputHandler, runner).RemoveAllListeners();
        // it's no longer controlled!
        GetComponent<CharacterSelectorHandler>(runner).ShowSelector(false);
    }

    private UnityEvent<InputAction.CallbackContext> GetMovementEvent(CharacterInputHandler scoutInputHandler, StateMachineRunner runner)
    {
        return GetComponent<CharacterControlledBy>(runner).Player == CharacterControlledBy.Players.P1
            ? scoutInputHandler.MovementP1
            : scoutInputHandler.MovementP2;
    }
}
