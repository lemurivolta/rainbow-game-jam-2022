using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAnimationsHandler : MonoBehaviour
{
    private Animator Animator;

    private CharacterControlledBy CharacterControlledBy;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        CharacterControlledBy = GetComponent<CharacterControlledBy>();
    }

    public bool UseInput { get; set; } = false;

    public void OnMovementP1(InputAction.CallbackContext context)
    {
        OnInputMovement(context, CharacterControlledBy.Players.P1);
    }

    public void OnMovementP2(InputAction.CallbackContext context)
    {
        OnInputMovement(context, CharacterControlledBy.Players.P2);
    }

    private void OnInputMovement(InputAction.CallbackContext context, CharacterControlledBy.Players player)
    {
        if (UseInput && CharacterControlledBy.Player == player)
        {
            SetMovementDirection(context.ReadValue<Vector2>());
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
}
