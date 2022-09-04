using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actionable : MonoBehaviour
{
    public UnityEvent ActedUpon;

    public void PerformAction()
    {
        ActedUpon.Invoke();
    }
}
