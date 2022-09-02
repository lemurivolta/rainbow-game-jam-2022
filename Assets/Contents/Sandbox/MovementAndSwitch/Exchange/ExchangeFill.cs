using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeFill : MonoBehaviour
{
    private SpriteMask SpriteMask;

    private void Start()
    {
        SpriteMask = GetComponent<SpriteMask>();
    }

    public void OnTimeoutProgress(float newValue)
    {
        SpriteMask.alphaCutoff = 1 - newValue;
    }
}
