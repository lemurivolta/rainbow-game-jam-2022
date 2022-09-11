using UnityEngine;

public class CharacterMovementHandler : MonoBehaviour
{
    [Tooltip("Speed of the character while moving")]
    public float Speed;

    private Rigidbody2D Rigidbody2D;

    private CharacterInfo CharacterInfo;

    private void Start()
    {
        CharacterInfo = GetComponent<CharacterInfo>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Called by the input system to set the movement of player 1.
    /// </summary>
    /// <param name="context">The input context.</param>
    public void OnMovementP1(Vector2 direction)
    {
        OnMovement(direction, CharacterInfo.Players.P1);
    }

    /// <summary>
    /// Called by the input system to set the movement of player 2.
    /// </summary>
    /// <param name="context">The input context.</param>
    public void OnMovementP2(Vector2 direction)
    {
        OnMovement(direction, CharacterInfo.Players.P2);
    }

    private void OnMovement(Vector2 direction, CharacterInfo.Players p)
    {
        if (//!Balloon.Instance.isBarking &&
            CharacterInfo != null &&
            !CharacterInfo.IsFollower &&
            CharacterInfo.Player == p)
        {
            Rigidbody2D.velocity = Speed * direction.normalized;
        }
    }

    /// <summary>
    /// Stop any current movement.
    /// </summary>
    public void StopMovement()
    {
        Rigidbody2D.velocity = Vector2.zero;
    }
}
