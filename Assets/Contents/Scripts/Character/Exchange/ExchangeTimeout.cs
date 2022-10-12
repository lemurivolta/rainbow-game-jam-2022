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

    bool exchangeP1Activated = false;
    bool exchangeP2Activated = false;

    public void OnExchangeCharactersStartedP1()
    {
        exchangeP1Activated = true;
    }

    public void OnExchangeCharactersStoppedP1()
    {
        exchangeP1Activated = false;
    }

    public void OnExchangeCharactersStartedP2()
    {
        exchangeP2Activated = true;
    }

    public void OnExchangeCharactersStoppedP2()
    {
        exchangeP2Activated = false;
    }

    private void Update()
    {
        if (AcceptingInput && exchangeP1Activated && exchangeP2Activated)
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
                exchangeP1Activated = false;
                exchangeP2Activated = false;
            }
        }
        else
        {
            ResetBeginExchangeTime();
        }
    }
}
