using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnknownAction : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;

    public float Duration;

    private Coroutine HideUnknownActionCoroutine;

    public void ShowUnknownAction()
    {
        SpriteRenderer.enabled = true;
        if (HideUnknownActionCoroutine != null)
        {
            StopCoroutine(HideUnknownActionCoroutine);
        }
        HideUnknownActionCoroutine = StartCoroutine(HideUnknownAction());
    }

    private IEnumerator HideUnknownAction()
    {
        yield return new WaitForSeconds(Duration);
        SpriteRenderer.enabled = false;
        HideUnknownActionCoroutine = null;
    }
}
