using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.Events;

public class ExchangeTimeout : MonoBehaviour
{
    public float TimeoutLength;

    private bool AcceptingInput;

    private float BeginExchangeTime = float.PositiveInfinity;

    private void ResetBeginExchangeTime()
    {
        BeginExchangeTime = float.PositiveInfinity;
        Progress.Invoke(0);
    }

    public UnityEvent<float> Progress;

    public VoidEvent ExchangePerformed;

    public void OnExchangeEnabled(bool enabled)
    {
        AcceptingInput = enabled;
    }

    bool switchP1Pressed = false;
    bool switchP2Pressed = false;

    public void OnSwitchCharactersP1Pressed()
    {
        switchP1Pressed = true;
    }

    public void OnSwitchCharactersP1Released()
    {
        switchP1Pressed = false;
    }

    public void OnSwitchCharactersP2Pressed()
    {
        switchP2Pressed = true;
    }

    public void OnSwitchCharactersP2Released()
    {
        switchP2Pressed = false;
    }

    private void Update()
    {
        if (AcceptingInput && (switchP1Pressed || switchP2Pressed))
        {
            if (BeginExchangeTime == float.PositiveInfinity)
            {
                BeginExchangeTime = Time.time;
            }
            var delta = Mathf.Min((Time.time - BeginExchangeTime) / TimeoutLength, 1);
            Progress.Invoke(delta);
            if (delta >= 1)
            {
                ExchangePerformed.Raise();
                ResetBeginExchangeTime();
            }
        }
        else
        {
            ResetBeginExchangeTime();
        }
    }
}
