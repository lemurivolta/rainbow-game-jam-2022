using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class State : MonoBehaviour
{
    public UnityEvent Changed;

    private bool _Value = false;

    public bool Value { get { return _Value; } }

    public void Set(bool newValue)
    {
        if (_Value != newValue)
        {
            _Value = newValue;
            Changed.Invoke();
        }
    }

    public void SetTrue() { Set(true); }

    public void SetFalse() { Set(false); }
}
