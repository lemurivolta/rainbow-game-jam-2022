using System;
using System.Collections;
using UnityEngine;

public class SpriteOverlap : MonoBehaviour
{
    private const float AlphaWhenSecondary = 100f / 255f;

    public SpriteRenderer SpriteRenderer;

    public bool isEnabled = true;

    public void SetIsEnabled(bool isEnabled)
    {
        this.isEnabled = isEnabled;
    }

    private string GetName(GameObject o)
    {
        return o.transform.parent.gameObject.name + "." + o.name;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isEnabled) { return; }
        Debug.Log($"{GetName(gameObject)} entered triggered of {GetName(other.gameObject)}");
        SetColor(other, AlphaWhenSecondary);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isEnabled) { return; }
        Debug.Log($"{GetName(gameObject)} exited triggered of {GetName(other.gameObject)}");
        SetColor(other, 1);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (!isEnabled) { return; }
        if(Time.frameCount % 60 == 0) {
            Debug.Log($"{GetName(gameObject)} in in trigger of {GetName(other.gameObject)}");
        }
    }

    private Coroutine colorAnimationCoroutine = null;

    private void SetColor(Collider2D other, float alpha)
    {
        var spriteOverlap = other.GetComponent<SpriteOverlap>();
        if (spriteOverlap == null || !spriteOverlap.isEnabled)
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
        while (Time.time <= start + animationDuration)
        {
            var t = (Time.time - start) / animationDuration;
            SpriteRenderer.color = Color.Lerp(color, newColor, t);
            yield return null;
        }
        colorAnimationCoroutine = null;
    }
}