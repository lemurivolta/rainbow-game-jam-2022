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
            animator.SetFloat("Direction",
                direction.x > 0 ? 1.0f :
                direction.y < 0 ? 2.0f :
                direction.x < 0 ? 3.0f :
                0.0f);
        }
        animator.SetBool("Walking", walking);
    }
}
