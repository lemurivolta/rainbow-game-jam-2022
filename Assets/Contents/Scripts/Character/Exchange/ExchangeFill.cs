using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeFill : MonoBehaviour
{
    private SpriteMask SpriteMask;

    private void Awake()
    {
        SpriteMask = GetComponent<SpriteMask>();
    }

    public void OnTimeoutProgress(float newValue)
    {
        if(SpriteMask != null)
            SpriteMask.alphaCutoff = 1 - newValue;
    }
}
