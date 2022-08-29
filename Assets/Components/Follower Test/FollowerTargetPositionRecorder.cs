using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that records the positions of the main character to follow its steps.
/// It keeps only a set amount of positions with a given minimum distance between them.
/// Notifies through events when we're starting to discard older positions.
/// </summary>
[RequireComponent(typeof(FollowerTracker))]
public class FollowerTargetPositionRecorder : MonoBehaviour
{
    [Tooltip("Minimum distance the target has to walk before a new step is recorded")]
    public float MinStepDistance = .3f;

    [Tooltip("Maximum number of steps saved")]
    public int MaxStepsNumber = 4;

    private List<Vector2> steps = new();

    private FollowerTracker followerTracker;

    public UnityEvent<Vector2> StepDiscarded = new();

    // Start is called before the first frame update
    void Start()
    {
        followerTracker = GetComponent<FollowerTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        // adds a step only if we have no steps, or if the target has walked far enough
        Vector2 newPosition = followerTracker.Target.transform.position;
        if (steps.Count == 0 || (steps[steps.Count - 1] - newPosition).sqrMagnitude > MinStepDistance)
        {
            steps.Add(newPosition);
        }
        // discard the oldest step if the target moved far enough
        if (steps.Count > MaxStepsNumber)
        {
            var discardedStep = steps[0];
            steps.RemoveAt(0);
            StepDiscarded.Invoke(discardedStep);
        }
    }
}
