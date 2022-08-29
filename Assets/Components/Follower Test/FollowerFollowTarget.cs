using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that makes a follower track its target keeping a specified distance, walking at the same speed as the target.
/// </summary>
[RequireComponent(typeof(FollowerTracker))]
[RequireComponent(typeof(FollowerTargetPositionRecorder))]
public class FollowerFollowTarget : MonoBehaviour
{
    [Tooltip("Invoked whenever the followers start to move to follow the target, with a direction vector parameter")]
    public UnityEvent<Vector2> StartMoving;

    [Tooltip("Invoked whenever the followers stops moving because it reached the target")]
    public UnityEvent StopMoving;

    private Coroutine MovementCoroutine = null;

    private Vector2? CurrentDestination = Vector2.positiveInfinity;

    private float speed;

    private FollowerTracker followerTracker;

    private FollowerTargetPositionRecorder followerTargetPositionRecorder;

    [Tooltip("Maximum distance allowed for the target, after which the follower starts moving")]
    public float MaxDistance = 1f;

    private void Start()
    {
        followerTargetPositionRecorder = GetComponent<FollowerTargetPositionRecorder>();
        // copy the speed from the target
        followerTracker = GetComponent<FollowerTracker>();
        speed = followerTracker.Target.GetComponent<ScoutMovementHandler>().Speed;
    }

    private void Update()
    {
        // run follow animation if the target went too far
        var targetPosition = (Vector2)followerTracker.Target.transform.position;
        var myPosition = (Vector2)transform.position;
        if ((targetPosition - myPosition).sqrMagnitude > MaxDistance * MaxDistance)
        {
            // look for the nearest step at given distance
            var steps = followerTargetPositionRecorder.Steps;
            var targetStep = followerTargetPositionRecorder.Steps
                .First(step => (targetPosition - step).sqrMagnitude < MaxDistance * MaxDistance);
            if (targetStep == CurrentDestination)
            {
                return;
            }
            // found it! start animation, stopping the previous one if necessary
            if (MovementCoroutine != null)
            {
                StopCoroutine(MovementCoroutine);
            }
            CurrentDestination = targetStep;
            MovementCoroutine = StartCoroutine(MoveTo(targetStep));
        }
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
            var t = (Time.time - startTime) / (endTime - startTime);
            Debug.Log($"lerping({t}) toward {endPosition}");
            transform.position = Vector3.Lerp(
                startPosition,
                endPosition,
                t
            );
            yield return null;
        }

        // at the end, forcibly set the destination
        transform.position = endPosition;

        // we finished!
        MovementCoroutine = null;
        CurrentDestination = Vector2.positiveInfinity;
        StopMoving.Invoke();
    }
}
