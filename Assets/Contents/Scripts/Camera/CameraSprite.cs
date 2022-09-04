using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSprite : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;

    public Sprite[] Sprites;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnAngleChanged(float x)
    {
        var i = Mathf.Clamp(
            (int)Mathf.Floor(Sprites.Length * (x + 1) / 2f),
            0,
            Sprites.Length - 1
        );
        SpriteRenderer.sprite = Sprites[i];
    }
}
