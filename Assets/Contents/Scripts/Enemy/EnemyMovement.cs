using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform Path;

    public float Speed = 1f;

    public float PauseTime = 0.5f;

    private int currentSegment = 0;

    private Rigidbody2D Rigidbody2D;

    private Vector3[] Points = null;

    private float PausedAt = -1;

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Points = new Vector3[Path.childCount];
        for (var i = 0; i < Path.childCount; i++)
        {
            Points[i] = Path.GetChild(i).position;
        }
        SetVelocity();
    }

    private void FixedUpdate()
    {
        // check if we are pausing
        if (PausedAt > 0)
        {
            //Debug.Log($"Check pause: {Time.time} - {PausedAt} >= {PauseTime}?");
            if (Time.time - PausedAt >= PauseTime)
            {
                //Debug.Log("Remove pause");
                PausedAt = -1;
                SetVelocity();
            }
            else
            {
                //Debug.Log("Still pausing");
                return;
            }
        }
        // check velocity along current path
        var p1 = GetPosition(currentSegment);
        var p2 = GetPosition(currentSegment + 1);
        var p = transform.position;
        if ((p - p1).sqrMagnitude >= (p2 - p1).sqrMagnitude)
        {
            // we are almost overshooting the destination (or already did): correct!
            currentSegment = (currentSegment + 1) % Points.Length;
            PausedAt = Time.time;
            Rigidbody2D.velocity = Vector3.zero;
        }
    }

    private void SetVelocity()
    {
        var p1 = GetPosition(currentSegment);
        var p2 = GetPosition(currentSegment + 1);
        Rigidbody2D.velocity = Speed * (p2 - p1).normalized;
    }

    private Vector3 GetPosition(int i)
    {
        if (Points == null || Points.Length == 0)
        {
            if (Path.childCount == 0)
            {
                return Vector3.zero;
            }
            else
            {
                return Path.GetChild(i % Path.childCount).position;
            }
        }
        else
        {
            return Points[i % Points.Length];
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (!Path || Path.childCount < 2)
        {
            return;
        }
        var firstPosition = GetPosition(0);
        var previousPosition = firstPosition;
        for (var i = 1; i < Path.childCount; i++)
        {
            var childPosition = GetPosition(i);
            Gizmos.DrawLine(previousPosition, childPosition);
            previousPosition = childPosition;
        }
        Gizmos.DrawLine(previousPosition, firstPosition);
    }

}
