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
        animator.SetFloat("DirectionX", direction.x);
        animator.SetFloat("DirectionY", direction.y);
        animator.SetBool("Walking", true);
    }

    public void OnStopMoving()
    {
        animator.SetBool("Walking", false);
    }
}
