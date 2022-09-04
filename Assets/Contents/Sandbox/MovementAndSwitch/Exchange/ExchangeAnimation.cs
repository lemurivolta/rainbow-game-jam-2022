using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeAnimation : MonoBehaviour
{
    private CharacterControlledBy CharacterControlledBy;

    public float Duration = 1f;

    private void Start()
    {
        CharacterControlledBy = GetComponent<CharacterControlledBy>();
    }

    public void OnExchangePerformed()
    {
        if (!CharacterControlledBy.IsFollower)
        {
            Destination = CharacterControlledBy.Companion.transform.position;
            StartCoroutine(StartExchangeAnimation());
        }
    }

    private Vector3 Destination;

    private IEnumerator StartExchangeAnimation()
    {
        var startPosition = transform.position;
        var startTime = Time.time;
        var endTime = startTime + Duration;
        for(; ; )
        {
            yield return null;

            if (Time.time >= endTime)
            {
                break;
            }
            transform.position = Vector3.Lerp(
                startPosition,
                Destination,
                (Time.time - startTime) / Duration
            );
        }
        transform.position = Destination;
    }
}
