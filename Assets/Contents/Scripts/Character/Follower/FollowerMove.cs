using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FollowerMove : MonoBehaviour
{
    [Tooltip("Invoked whenever the followers start to move to follow the target, with a direction vector parameter")]
    public UnityEvent<Vector2> StartMoving;

    [Tooltip("Invoked whenever the followers stops moving because it reached the target")]
    public UnityEvent StopMoving;

    private Coroutine MovementCoroutine = null;

    private Vector2 StartPosition = Vector2.positiveInfinity;

    private Vector2 EndPosition = Vector2.positiveInfinity;

    private float StartTime = 0;

    private float Speed;

    /// <summary>
    /// Start a movement to the given destination at given speed, or updates
    /// the destination and speed if it's already moving.
    /// </summary>
    /// <param name="destination">The destination to reach.</param>
    /// <param name="speed">The speed to reach the destination.</param>
    public void MoveTo(Vector2 destination, float speed)
    {
        // if no movement parameter has changed, keep doing what we're doing
        if (EndPosition == destination && Speed == speed)
        {
            return;
        }
        // update movement parameters
        StartPosition = MovementCoroutine == null ? transform.position : PositionAtCurrentTime;
        EndPosition = destination;
        StartTime = Time.time;
        Speed = speed;
        // otherwise, start the coroutine if it was stopped
        if (MovementCoroutine == null)
        {
            MovementCoroutine = StartCoroutine(MoveTo());
        }
        // inform listeners
        StartMoving.Invoke(EndPosition - StartPosition);
    }

    /// <summary>
    /// Stop all the currently ongoing movements.
    /// </summary>
    public void Stop()
    {
        if (MovementCoroutine != null)
        {
            StopCoroutine(MovementCoroutine);
            MovementCoroutine = null;
            StopMoving.Invoke();
        }
    }

    /// <summary>
    /// The position at Time.time.
    /// </summary>
    private Vector2 PositionAtCurrentTime
    {
        get => StartPosition +
            (Time.time - StartTime) * Speed *
            (EndPosition - StartPosition).normalized;
    }

    /// <summary>
    /// Coroutine to move the game object at every frame and then stop once reached the destination.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveTo()
    {
        // update position every frame
        for (; ; )
        {
            // check if we reached the end
            var endTime = StartTime + (EndPosition - StartPosition).sqrMagnitude / Speed;
            if (Time.time >= endTime)
            {
                break;
            }
            // if not, update the position and wait for a frame
            transform.position = PositionAtCurrentTime;
            yield return null;
        }

        // at the end, forcibly set the end destination to avoid rounding errors
        transform.position = EndPosition;

        // we finished!
        MovementCoroutine = null;
        StopMoving.Invoke();
    }
}
