using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraRotation : MonoBehaviour
{
    public bool DoSweep = true;
    public float MinAngle;
    public float MaxAngle;
    public float StartingAngle;
    public float SingleSweepDuration;
    public float WaitTimeAtSweepEnd;

    [Tooltip("Raise an event with value from -1 to 1 whenever the angle changes from -MinAngle to +MaxAngle")]
    public UnityEvent<float> AngleChanged;

    private Coroutine SweepCoroutine = null;

    private void Start()
    {
        CheckCoroutine();
    }

    private void Update()
    {
        CheckCoroutine();
    }

    private void CheckCoroutine()
    {
        if (DoSweep && SweepCoroutine == null)
        {
            SweepCoroutine = StartCoroutine(DoMovement());
        }
        if(!DoSweep && SweepCoroutine != null)
        {
            StopCoroutine(SweepCoroutine);
        }
    }

    public void AnimateToAngle(float value, float duration)
    {
        DoSweep = false;
        StartCoroutine(AnimateToAngleCoroutine(value, duration));
    }

    IEnumerator DoMovement()
    {
        // fake start time to simulate the fact we're in the middle of the roration
        var startTime = Time.time -
            SingleSweepDuration * (StartingAngle - MinAngle) / (MaxAngle - MinAngle);
        SetRotation(StartingAngle);
        var angle1 = MinAngle;
        var angle2 = MaxAngle;
        for (; ; )
        {
            for(; ; )
            {
                var delta = (Time.time - startTime) / SingleSweepDuration;
                if(delta >= 1)
                {
                    yield return new WaitForSeconds(WaitTimeAtSweepEnd);
                    break;
                }
                SetRotation(Mathf.Lerp(angle1, angle2, delta));
                yield return null;
            }
            (angle2, angle1) = (angle1, angle2);
            startTime = startTime + SingleSweepDuration + WaitTimeAtSweepEnd;
        }
    }

    private IEnumerator AnimateToAngleCoroutine(float destinationAngle, float duration)
    {
        var startAngle = transform.rotation.eulerAngles.z;
        var startTime = Time.time;
        for(; ; )
        {
            var delta = (Time.time - startTime) / duration;
            if(delta >= 1)
            {
                break;
            }
            var angle = Mathf.Lerp(startAngle, destinationAngle, delta);
            SetRotation(angle);
            yield return null;
        }
        SetRotation(destinationAngle);
    }

    private void SetRotation(float angle)
    {
        var value = 2 * (angle - MinAngle) / (MaxAngle - MinAngle) - 1;
        AngleChanged.Invoke(value);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
