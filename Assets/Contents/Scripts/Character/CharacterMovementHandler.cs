using SimpleStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovementHandler : MonoBehaviour
{
    [Tooltip("Speed of the character while moving")]
    public float Speed;

    private Vector2 VelocityP1 = Vector2.zero;

    private Vector2 VelocityP2 = Vector2.zero;

    private BoxCollider2D BoxCollider2D;

    private CharacterControlledBy CharacterControlledBy;

    private void Start()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        CharacterControlledBy = GetComponent<CharacterControlledBy>();
    }

    /// <summary>
    /// Called by the input system to set the movement of player 1.
    /// </summary>
    /// <param name="context">The input context.</param>
    public void OnMovementP1(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        VelocityP1 = Speed * direction.normalized;
    }

    /// <summary>
    /// Called by the input system to set the movement of player 2.
    /// </summary>
    /// <param name="context">The input context.</param>
    public void OnMovementP2(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        VelocityP2 = Speed * direction.normalized;
    }

    /// <summary>
    /// Stop any current movement.
    /// </summary>
    public void StopMovement()
    {
        VelocityP2 = Vector2.zero;
    }

    /// <summary>
    /// Layer mask to check during collisions.
    /// </summary>
    public LayerMask CollisionMask;

    /// <summary>
    /// Rotation to try out in order to find if there's a way to "slide" against objects
    /// </summary>
    private float[] TentativeRotations = new float[]
    {
        0,
        Mathf.PI/4,
        -Mathf.PI/4
    };

    private void FixedUpdate()
    {
        // get the correct velocity according to which player controls us
        var velocity = CharacterControlledBy.Player == CharacterControlledBy.Players.P1
            ? VelocityP1
            : VelocityP2;
        // don't do anything if not moving
        if (velocity.x == 0 && velocity.y == 0)
        {
            return;
        }
        // check if we will collide with something else, trying all the (projected) directions
        var delta = Time.fixedDeltaTime * velocity;
        foreach (var rotation in TentativeRotations)
        {
            var actualDelta = RotateAndProject(rotation, delta);
            var center = (Vector2)transform.position + BoxCollider2D.offset + actualDelta;
            var collider = Physics2D.OverlapBox(center, BoxCollider2D.size, 0, CollisionMask);
            if (collider == null)
            {
                // no collision: apply movement and stop searching for a direction
                transform.position += (Vector3)actualDelta;
                break;
            }
        }
    }

    /// <summary>
    /// Project a vector over another vector at given rotation.
    /// </summary>
    /// <param name="rotation">The rotation of the final vector.</param>
    /// <param name="v">The original vector.</param>
    /// <returns>The projection.</returns>
    private Vector2 RotateAndProject(float rotation, Vector2 v)
    {
        // given vector d, a vector r at an angle a which is d's projection,
        // r = cos a * ( rotation(a) * d )
        var cos = Mathf.Cos(rotation);
        var sin = Mathf.Sin(rotation);
        return cos * new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }
}
