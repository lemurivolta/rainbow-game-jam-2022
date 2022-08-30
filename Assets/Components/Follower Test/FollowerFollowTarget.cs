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

    private float speed;
    private FollowerTracker followerTracker;

    private FollowerTargetPositionRecorder followerTargetPositionRecorder;

    [Tooltip("Maximum distance allowed for the target, after which the follower starts moving")]
    public float MaxDistance = 1f;

    [Tooltip("Number of frames to wait before declaring that the movement has actually stopped")]
    public int FrameTolerance = 5;

    private void Start()
    {
        // get components we'll reference during execution
        followerTargetPositionRecorder = GetComponent<FollowerTargetPositionRecorder>();
        followerTracker = GetComponent<FollowerTracker>();
        // copy the speed from the target
        speed = followerTracker.Target.GetComponent<ScoutMovementHandler>().Speed;
    }

    private void Update()
    {
        // run follow animation if the target went too far
        var targetPosition = (Vector2)followerTracker.Target.transform.position;
        var myPosition = (Vector2)transform.position;
        var sqrMaxDistance = MaxDistance * MaxDistance;
        if ((targetPosition - myPosition).sqrMagnitude >= sqrMaxDistance)
        {
            // find the destination position
            var steps = followerTargetPositionRecorder.Steps;
            var targetStep = steps[0];
            MoveTo(targetStep);
        }
    }

    #region movement coroutine

    private Coroutine MovementCoroutine = null;

    private float TimeStart = 0;
    private Vector2 MovementStart = Vector2.positiveInfinity;
    private Vector2 MovementEnd = Vector2.positiveInfinity;

    private void MoveTo(Vector2 destination)
    {
        // nothing to do if the destination is our current value
        if (MovementEnd == destination)
        {
            return;
        }
        StopDeclareMovementFinishedCoroutine();
        // update movement parameters
        MovementStart = MovementCoroutine == null ? transform.position : GetCurrentMovementValue();
        MovementEnd = destination;
        TimeStart = Time.time;
        // start the coroutine if it was stopped
        if (MovementCoroutine == null)
        {
            Debug.Log($"started at frame {Time.frameCount}");
            MovementCoroutine = StartCoroutine(MoveTo());
        }
        // inform listeners of the movement
        StartMoving.Invoke(MovementEnd - MovementStart);
    }

    private Vector2 GetCurrentMovementValue()
    {
        return MovementStart + (MovementEnd - MovementStart).normalized * (Time.time - TimeStart) * speed;
    }

    private IEnumerator MoveTo()
    {
        // update position every frame
        for (; ; )
        {
            // check if we reached the end
            var timeEnd = TimeStart + (MovementEnd - MovementStart).sqrMagnitude / speed;
            if (Time.time >= timeEnd)
            {
                break;
            }
            // if not, update the position and wait for a frame
            transform.position = GetCurrentMovementValue();
            yield return null;
        }

        // at the end, forcibly set the end destination
        transform.position = MovementEnd;

        // we finished!
        MovementCoroutine = null;
        StartDeclareMovementFinishedCoroutine();
    }

    private Coroutine DeclareMovementFinishedCoroutine = null;

    private void StartDeclareMovementFinishedCoroutine()
    {
        StopDeclareMovementFinishedCoroutine();
        DeclareMovementFinishedCoroutine = StartCoroutine(DeclareMovementFinished());
    }

    private void StopDeclareMovementFinishedCoroutine()
    {
        if (DeclareMovementFinishedCoroutine != null)
        {
            StopCoroutine(DeclareMovementFinishedCoroutine);
            DeclareMovementFinishedCoroutine = null;
        }
    }

    private IEnumerator DeclareMovementFinished()
    {
        var numFrames = Time.frameCount;
        while (Time.frameCount < numFrames + FrameTolerance)
        {
            yield return null;
        }
        Debug.Log($"finished at frame {Time.frameCount}");
        StopMoving.Invoke();
        DeclareMovementFinishedCoroutine = null;
    }

    #endregion
}
