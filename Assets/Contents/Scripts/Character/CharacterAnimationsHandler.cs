using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAnimationsHandler : MonoBehaviour
{
    private Animator Animator;

    private CharacterInfo CharacterInfo;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        CharacterInfo = GetComponent<CharacterInfo>();
    }

    public bool UseInput { get; set; } = false;

    public void OnMovementP1(Vector2 direction)
    {
        OnInputMovement(direction, CharacterInfo.Players.P1);
    }

    public void OnMovementP2(Vector2 direction)
    {
        OnInputMovement(direction, CharacterInfo.Players.P2);
    }

    private void OnInputMovement(Vector2 direction, CharacterInfo.Players player)
    {
        if (UseInput && CharacterInfo.Player == player)
        {
            SetMovementDirection(direction);
        }
    }

    public void SetMovementDirection(Vector2 direction)
    {
        var walking = !Mathf.Approximately(direction.x, 0) ||
            !Mathf.Approximately(direction.y, 0);
        if (walking)
        {
            Animator.SetFloat("DirectionX", direction.x);
            Animator.SetFloat("DirectionY", direction.y);
        }
        Animator.SetBool("Walking", walking);
    }

    public void StopMovement()
    {
        SetMovementDirection(Vector2.zero);
    }

    public void OnAction()
    {
        Animator.SetTrigger("Act");
    }
}
