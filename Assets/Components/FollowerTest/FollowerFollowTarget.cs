using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that follows the target, leveraging the target step tracking of FollowerTargetPositionRecorder,
/// and the ability to move of FollowerMove.
/// </summary>
[RequireComponent(typeof(FollowerTracker))]
[RequireComponent(typeof(FollowerTargetPositionRecorder))]
[RequireComponent(typeof(FollowerMove))]
public class FollowerFollowTarget : MonoBehaviour
{
    [Tooltip("Invoked whenever the followers stops moving because it reached the target, and the given tolerance in frames have been considered")]
    public UnityEvent StoppedMovingAndTolerance;

    private float speed;
    private FollowerTracker followerTracker;

    private FollowerTargetPositionRecorder followerTargetPositionRecorder;

    private FollowerMove followerMove;

    [Tooltip("Maximum distance allowed for the target, after which the follower starts moving")]
    public float MaxDistance = 1f;

    private void Start()
    {
        // get components we'll reference during execution
        followerTargetPositionRecorder = GetComponent<FollowerTargetPositionRecorder>();
        followerTracker = GetComponent<FollowerTracker>();
        followerMove = GetComponent<FollowerMove>();
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
            var destination = steps[0];
            // start the movement
            followerMove.MoveTo(destination, speed);
        }
    }

    public void OnStartMoving()
    {
        StopDeclareMovementFinishedCoroutine();
    }

    public void OnStopMoving()
    {
        StartDeclareMovementFinishedCoroutine();
    }

    #region movement frame tolerance
    /**
     * Because of rounding and of the discontinuous nature of the step algorithm, the normal
     * behaviour causes the follower to continuously start and stop movement. Because of this,
     * we add some frames of tolerance at the end of the movement before declaring the movement
     * actually stopped.
     */

    [Tooltip("Number of frames to wait before declaring that the movement has actually stopped")]
    public int FrameTolerance = 5;

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
        StoppedMovingAndTolerance.Invoke();
        DeclareMovementFinishedCoroutine = null;
    }

    #endregion
}
