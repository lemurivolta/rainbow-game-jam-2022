using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that records the positions of the main character to follow its steps.
/// It keeps only a set amount of positions with a given minimum distance between them.
/// Notifies through events when we're starting to discard older positions.
/// </summary>
[RequireComponent(typeof(CharacterInfo))]
public class FollowerTargetPositionRecorder : MonoBehaviour
{
    [Tooltip("Minimum distance the target has to walk before a new step is recorded")]
    public float MinStepDistance = .3f;

    [Tooltip("Maximum number of steps saved")]
    public int MaxStepsNumber = 4;

    private List<Vector2> steps = new();

    public IList<Vector2> Steps { get { return steps.AsReadOnly(); } }

    private CharacterInfo CharacterInfo;

    private void OnEnable()
    {
        SetupCharacterInfo();
    }

    private void OnDisable()
    {
        steps.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (steps.Count == 0)
        {
            // this happens on the first update after creation or after being disabled-enabled
            // (e.g.: character switch)
            // better do it in update because it relies on all the characters being correctly set,
            // and during onenable could be too early
            // make a fake history of steps from our position to the target
            var delta = 1f / (MaxStepsNumber - 1);
            var companion = CharacterInfo.Companion;
            var companionPosition = companion.transform.position;
            for (var i = 0; i < MaxStepsNumber; i++)
            {
                steps.Add(Vector2.Lerp(
                    transform.position,
                    companionPosition,
                    i * delta
                ));
            }
        }
        // adds a step only if we have no steps, or if the target has walked far enough
        Vector2 newPosition = CharacterInfo.Companion.transform.position;
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

    private List<Vector2> Stash;

    public void SetStashFromCorrespondent()
    {
        SetupCharacterInfo();
        Stash = CharacterInfo.Correspondent
            .GetComponent<FollowerTargetPositionRecorder>()
            .steps;
    }

    public void ApplyStash()
    {
        steps = Stash;
    }

    private void SetupCharacterInfo()
    {
        CharacterInfo = CharacterInfo != null ? CharacterInfo : GetComponent<CharacterInfo>();
    }
}
