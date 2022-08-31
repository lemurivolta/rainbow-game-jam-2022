using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FollowingState", menuName = "Simple State Machine/MAS/Create Following State Hooks")]
public class FollowingState : StateHooks
{
    public override void OnEnter(StateMachineRunner runner)
    {
        Debug.Log("Following: enter", runner);
        // set up the follower components to track the companion
        GetComponent<FollowerTracker>(runner).Target =
            GetComponent<CharacterControlledBy>(runner).GetCompanion();
        GetComponent<FollowerTargetPositionRecorder>(runner).enabled = true;
        GetComponent<FollowerFollowTarget>(runner).enabled = true;
        // stop handling physics
        GetComponent<BoxCollider2D>(runner).enabled = false;
        GetComponent<CharacterMovementHandler>(runner).StopMovement();
    }

    public override void OnExit(StateMachineRunner runner)
    {
        Debug.Log("Following: exit", runner);
        // stops the follower scripts, and interrupts ongoing movements
        GetComponent<FollowerTargetPositionRecorder>(runner).enabled = false;
        GetComponent<FollowerFollowTarget>(runner).enabled = false;
        GetComponent<FollowerMove>(runner).Stop();
        // resume handling physics
        GetComponent<BoxCollider2D>(runner).enabled = true;
    }
}
