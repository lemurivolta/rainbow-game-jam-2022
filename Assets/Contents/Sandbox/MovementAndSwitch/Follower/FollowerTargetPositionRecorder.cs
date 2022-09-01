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

    public IList<Vector2> Steps { get { return steps.AsReadOnly(); } }

    private void OnEnable()
    {
        followerTracker = followerTracker != null ? followerTracker : GetComponent<FollowerTracker>();
        // make a fake history of steps from our position to the target
        for(var i = 0; i < MaxStepsNumber; i++)
        {
            steps.Add(Vector2.Lerp(
                transform.position,
                followerTracker.Target.transform.position,
                (float)i / (MaxStepsNumber - 1)
            ));
        }
    }

    private void OnDisable()
    {
        steps.Clear();
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
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var step in steps)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(step, 0.1f);
        }
    }
}
