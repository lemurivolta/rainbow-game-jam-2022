using System;
using System.Collections;
using UnityEngine;

public class DoorMarkers : MonoBehaviour
{
    [SerializeField] private SpriteMask MaskP1;
    [SerializeField] private SpriteMask MaskP2;

    private int counterP1 = 0;
    private int counterP2 = 0;

    public void OnPlayerApproachedControl(CharacterInfo.Players player)
    {
        switch (player)
        {
            case CharacterInfo.Players.P1: counterP1++; break;
            case CharacterInfo.Players.P2: counterP2++; break;
        }
        UpdateAnimation();
    }

    public void OnPlayerLeftControl(CharacterInfo.Players player)
    {
        switch (player)
        {
            case CharacterInfo.Players.P1: counterP1--; break;
            case CharacterInfo.Players.P2: counterP2--; break;
        }
        UpdateAnimation();
    }

    private Coroutine animationCoroutineP1 = null;
    private Coroutine animationCoroutineP2 = null;

    private void UpdateAnimation()
    {
        UpdateAnimation(counterP1, animationCoroutineP1, value => { animationCoroutineP1 = value; }, MaskP1);
        UpdateAnimation(counterP2, animationCoroutineP2, value => { animationCoroutineP2 = value; }, MaskP2);
    }

    private void UpdateAnimation(int counter, Coroutine animationCoroutine,
        Action<Coroutine> setAnimationCoroutine, SpriteMask mask)
    {
        if (counter > 0 && animationCoroutine == null)
        {
            setAnimationCoroutine(StartCoroutine(Animate(mask)));
        }
        if (counter <= 0)
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                setAnimationCoroutine(null);
            }
            mask.alphaCutoff = 1;
        }
    }

    [SerializeField] private float animationDuration = 0.5f;

    private IEnumerator Animate(SpriteMask mask)
    {
        var startTime = Time.time;
        while (Time.time < startTime + animationDuration)
        {
            mask.alphaCutoff = Mathf.Lerp(1, 0, (Time.time - startTime) / animationDuration);
            yield return null;
        }
        mask.alphaCutoff = 0;
    }
}
