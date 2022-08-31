using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovementHandler : MonoBehaviour
{
    [Tooltip("Speed of the character while moving")]
    public float Speed;

    private Vector2 Velocity = Vector2.zero;

    private BoxCollider2D BoxCollider2D;

    private void Start()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Called by the input system to set a movement direction.
    /// </summary>
    /// <param name="context">The input context.</param>
    public void OnMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        Velocity = Speed * direction.normalized;
    }

    /// <summary>
    /// Stop any current movement.
    /// </summary>
    public void StopMovement()
    {
        Velocity = Vector2.zero;
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
        if (Velocity.x == 0 && Velocity.y == 0)
        {
            // not moving, do nothing
            return;
        }
        // check if we will collide with something else, trying all the directions
        var delta = Time.fixedDeltaTime * Velocity;
        Debug.Log($"Checking for base {delta}");
        // try also skewed (projected) directions in order to "slide" against walls
        foreach (var rotation in TentativeRotations)
        {
            var actualDelta = RotateAndProject(rotation, delta);
            Debug.Log($"Checking for {actualDelta}");
            var center = (Vector2)transform.position + BoxCollider2D.offset + actualDelta;
            // we collide with the common objects and the other player
            var collider = Physics2D.OverlapBox(center, BoxCollider2D.size, 0, CollisionMask);
            if (collider == null)
            {
                // no collision: apply movement and stop.
                Debug.Log("Good!");
                transform.position += (Vector3)actualDelta;
                break;
            }
        }
    }

    /// <summary>
    /// Check if a collision happens when we move in a given direction.
    /// </summary>
    /// <param name="delta">The delta of movement to apply before checking.</param>
    /// <returns>Whether the collision happens.</returns>
    private bool Collides(Vector2 delta)
    {
        var center = (Vector2)transform.position + BoxCollider2D.offset + delta;
        var collider = Physics2D.OverlapBox(center, BoxCollider2D.size, 0, CollisionMask);
        if (collider != null)
        {
            // collision detected
            return true;
        }
        return false;
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
