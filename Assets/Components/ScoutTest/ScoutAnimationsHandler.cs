using UnityEngine;
using UnityEngine.InputSystem;

public class ScoutAnimationsHandler : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        var walking = direction.x != 0 || direction.y != 0;
        if (walking)
        {
            animator.SetFloat("Direction", direction.ToDirectionFloat());
        }
        animator.SetBool("Walking", walking);
    }
}
