using System;
using System.Collections;
using UnityEngine;

public class SpriteOverlap : MonoBehaviour
{
    private const float AlphaWhenSecondary = 100f / 255f;

    public SpriteRenderer SpriteRenderer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<SpriteOverlap>() == null)
        {
            SetColor(other, AlphaWhenSecondary);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<SpriteOverlap>() == null)
        {
            SetColor(other, 1);
        }
    }

    private Coroutine colorAnimationCoroutine = null;

    private void SetColor(Collider2D other, float alpha)
    {
        // only secondary elements have SpriteOverlap attached
        if (other.GetComponent<SpriteOverlap>() == null)
        {
            if (colorAnimationCoroutine != null)
            {
                StopCoroutine(colorAnimationCoroutine);
            }
            var color = SpriteRenderer.color;
            var newColor = new Color(color.r, color.g, color.b, alpha);
            colorAnimationCoroutine = StartCoroutine(AnimateColor(color, newColor));
        }
    }

    private const float animationDuration = 0.5f;

    private IEnumerator AnimateColor(Color color, Color newColor)
    {
        var start = Time.time;
        while(Time.time <= start + animationDuration)
        {
            var t = (Time.time - start) / animationDuration;
            SpriteRenderer.color = Color.Lerp(color, newColor, t);
            yield return null;
        }
        colorAnimationCoroutine = null;
    }
}