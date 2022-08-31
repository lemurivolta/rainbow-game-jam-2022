using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAnimationsHandler : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        var walking = !Mathf.Approximately(direction.x, 0) ||
            !Mathf.Approximately(direction.y, 0);
        if (walking)
        {
            animator.SetFloat("DirectionX", direction.x);
            animator.SetFloat("DirectionY", direction.y);
        }
        animator.SetBool("Walking", walking);
    }
}
