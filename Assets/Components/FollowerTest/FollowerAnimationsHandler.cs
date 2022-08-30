using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerAnimationsHandler : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnStartMoving(Vector2 direction)
    {
        animator.SetFloat("Direction", direction.ToDirectionFloat());
        animator.SetBool("Walking", true);
    }

    public void OnStopMoving()
    {
        animator.SetBool("Walking", false);
    }
}
