using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangeRotation : MonoBehaviour
{
    public float Angle;

    public float Duration;

    public CameraRotation CameraRotation;

    public void ChangeRotation()
    {
        CameraRotation.AnimateToAngle(Angle, Duration);
    }
}
