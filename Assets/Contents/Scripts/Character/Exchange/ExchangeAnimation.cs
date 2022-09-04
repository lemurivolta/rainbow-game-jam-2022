using System.Collections;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class ExchangeAnimation : MonoBehaviour
{
    private CharacterInfo CharacterInfo;

    public VoidEvent ExchangeAnimationFinished;

    public float Duration = 1f;

    private void Start()
    {
        CharacterInfo = GetComponent<CharacterInfo>();
    }

    public void OnExchangePerformed()
    {
        if (!CharacterInfo.IsFollower)
        {
            Destination = CharacterInfo.Correspondent.transform.position;
            StartCoroutine(StartExchangeAnimation());
        }
    }

    private Vector3 Destination;

    private static int NumAnimationsFinished = 0;

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
        // there are always 2 exchange animations finishing together
        NumAnimationsFinished++;
        if (NumAnimationsFinished == 2)
        {
            NumAnimationsFinished = 0;
            ExchangeAnimationFinished.Raise();
        }
    }
}
