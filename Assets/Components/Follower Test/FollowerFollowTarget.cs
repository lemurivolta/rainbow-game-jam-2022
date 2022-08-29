using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FollowerTracker))]
public class FollowerFollowTarget : MonoBehaviour
{
    [Tooltip("Invoked whenever the followers start to move to follow the target, with a direction vector parameter")]
    public UnityEvent<Vector2> StartMoving;

    [Tooltip("Invoked whenever the followers stops moving because it reached the target")]
    public UnityEvent StopMoving;

    private Coroutine MovementCoroutine = null;

    private float speed;

    private void Start()
    {
        // copy the speed from the target
        speed = GetComponent<FollowerTracker>().Target
            .GetComponent<ScoutMovementHandler>().Speed;
    }

    public void OnStepDiscarded(Vector2 discardedStep)
    {
        // don't start any animation if we are already there
        var totalDistance = (discardedStep - (Vector2)transform.position).sqrMagnitude;
        // Debug.Log($"from {(Vector2)transform.position} to {discardedStep} totalDistance: {totalDistance}");
        if (Mathf.Approximately(totalDistance, 0f))
        {
            return;
        }
        // stop previous animation if necessary
        if (MovementCoroutine != null)
        {
            StopCoroutine(MovementCoroutine);
        }
        // start animation
        MovementCoroutine = StartCoroutine(MoveTo(discardedStep));
    }

    private IEnumerator MoveTo(Vector2 discardedStep)
    {
        // compute start and end data
        var startPosition = transform.position;
        var startTime = Time.time;

        Vector3 endPosition = discardedStep;
        var endTime = startTime + (endPosition - startPosition).magnitude / speed;

        // inform listeners that we start moving
        StartMoving.Invoke(endPosition - startPosition);

        // update position every frame
        for (; ; )
        {
            if (Time.time >= endTime)
            {
                break;
            }
            transform.position = Vector3.Lerp(
                startPosition,
                endPosition,
                (Time.time - startTime) / (endTime - startTime)
            );
            yield return null;
        }

        // at the end, forcibly set the destination
        transform.position = endPosition;

        // we finished!
        MovementCoroutine = null;
        StopMoving.Invoke();
    }
}
