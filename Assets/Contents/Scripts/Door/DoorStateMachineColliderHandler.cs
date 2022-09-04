using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStateMachineColliderHandler : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var collider = animator.gameObject.GetComponent<Collider2D>();
        collider.enabled = !animator.GetBool("Opened");
    }
}
