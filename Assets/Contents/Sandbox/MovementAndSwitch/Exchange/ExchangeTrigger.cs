using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;

public class ExchangeTrigger : MonoBehaviour
{
    public BoolEvent ExchangeEnabledEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ExchangeEnabledEvent.Raise(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExchangeEnabledEvent.Raise(false);
    }
}
