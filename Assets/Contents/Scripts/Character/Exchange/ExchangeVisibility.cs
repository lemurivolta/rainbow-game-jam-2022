using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeVisibility : MonoBehaviour
{
    public float ReferenceWidth = 1.2f;

    private SpriteRenderer SpriteRenderer;
    private bool SpriteRendererEnabled = false;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisibility();
    }

    public void ChangeExchangeVisibility(bool visible)
    {
        SpriteRendererEnabled = visible;
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (SpriteRenderer != null)
        {
            SpriteRenderer.enabled = SpriteRendererEnabled;
            UpdatePositionAndSize();
        }
    }

    private void Update()
    {
        UpdatePositionAndSize();
    }

    private void UpdatePositionAndSize()
    {
        if(!SpriteRenderer.enabled)
        {
            return;
        }
        // find he root transforms of the two main characters
        Transform p1 = null, p2 = null;
        foreach(var ccb in CharacterInfo.AllCharacterControlledBy)
        {
            if(!ccb.IsFollower)
            {
                if(ccb.Player == CharacterInfo.Players.P1)
                {
                    p1 = ccb.transform;
                } else
                {
                    p2 = ccb.transform;
                }
            }
        }
        if(!p1 || !p2)
        {
            throw new System.Exception("Cannot find main characters");
        }
        // move the arrow between them
        var deltaPosition = p2.position - p1.position;
        var width = deltaPosition.magnitude;
        var scale = width / ReferenceWidth;
        var rotation = Mathf.Atan2(deltaPosition.y, deltaPosition.x);
        var position = (p1.position + p2.position) / 2;

        transform.position = position;
        transform.localScale= new Vector2(scale, scale);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * rotation);
    }
}
