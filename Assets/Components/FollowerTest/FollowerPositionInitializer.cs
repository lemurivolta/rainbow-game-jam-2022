using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that initialize the position of the follower and moves it under its target.
/// </summary>
[RequireComponent(typeof(FollowerTracker))]
public class FollowerPositionInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = GetComponent<FollowerTracker>().Target.transform.position +
            0.01f * Vector3.up;
    }

}
