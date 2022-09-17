using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNGDestination : MonoBehaviour
{
    public float MovementDuration = 1;

    public float ComeBackAfter = 3;

    public CHARACTER CharacterAllowedToMove = CHARACTER.YELENA;

    private CharacterInfo AllowedCharacterInfo = null;

    public void OnCharacterEnter(Collider2D collider)
    {
        var ci = collider.gameObject.transform.parent.gameObject.GetComponent<CharacterInfo>();
        if (ci.Character == CharacterAllowedToMove)
        {
            AllowedCharacterInfo = ci;
        }
    }

    public void OnCharacterExit(Collider2D collider)
    {
        var ci = collider.gameObject.transform.parent.gameObject.GetComponent<CharacterInfo>();
        if (ci.Character == CharacterAllowedToMove)
        {
            AllowedCharacterInfo = null;
        }
    }

    public void OnCharacterActionP1()
    {
        OnCharacterAction(CharacterInfo.Players.P1);
    }

    public void OnCharacterActionP2()
    {
        OnCharacterAction(CharacterInfo.Players.P2);
    }

    private bool Moving = false;

    public void OnCharacterAction(CharacterInfo.Players p)
    {
        // must check that we are not already moving, yelena is inside,
        // it's not a follower, and her player was the one issuing the command
        if (!Moving &&
            AllowedCharacterInfo != null &&
            !AllowedCharacterInfo.IsFollower &&
            AllowedCharacterInfo.Player == p)
        {
            StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        Moving = true;
        var targetTransform = transform.parent.transform;
        var pointA = targetTransform.position;
        var pointB = pointA + transform.localPosition;
        // move to destination
        yield return Move(targetTransform, pointB);
        // wait a bit
        yield return new WaitForSeconds(ComeBackAfter);
        // come back
        yield return Move(targetTransform, pointA);
        Moving = false;
    }

    private IEnumerator Move(Transform transform, Vector3 endPosition)
    {
        // move from current position to end position
        var startTime = Time.time;
        var startPosition = transform.position;
        for (; ; )
        {
            var k = (Time.time - startTime) / MovementDuration;
            if (k >= 1)
            {
                break;
            }
            transform.position = Vector3.Lerp(
                startPosition,
                endPosition,
                k
            );
            yield return null;
        }
        transform.position = endPosition;
    }

    private void OnDrawGizmos()
    {
        var startPosition = transform.parent.transform.position;
        var endPosition = transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPosition, endPosition);
    }
}
