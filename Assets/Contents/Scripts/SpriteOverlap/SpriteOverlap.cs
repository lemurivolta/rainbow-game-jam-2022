using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpriteOverlap : MonoBehaviour
{
    private const float AlphaWhenSecondary = 100f / 255f;

    public SpriteRenderer SpriteRenderer;

    public Collider2D Collider;

    public int priority = 0;

    public void SetPriority(int priority)
    {
        this.priority = priority;
    }

    private string GetName(GameObject o)
    {
        return o.transform.parent.gameObject.name + "." + o.name;
    }

    private HashSet<SpriteOverlap> trackedGameObjects = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        var spriteOverlap = other.GetComponent<SpriteOverlap>();
        Assert.IsNotNull(spriteOverlap);
        trackedGameObjects.Add(spriteOverlap);
        // PrintTracked();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var spriteOverlap = other.GetComponent<SpriteOverlap>();
        Assert.IsNotNull(spriteOverlap);
        trackedGameObjects.Remove(spriteOverlap);
        // PrintTracked();
    }

    // private void PrintTracked()
    // {
    //     var s = $"Tracked by {GetName(gameObject)}: ";
    //     foreach (var o in trackedGameObjects)
    //     {
    //         s += $"{GetName(o.gameObject)}, ";
    //     }
    //     Debug.Log(s);
    // }

    private bool covered = false;

    private static bool IsOver(SpriteRenderer sr1, SpriteRenderer sr2)
    {
        // see: https://docs.unity3d.com/Manual/2DSorting.html
        // first check by sorting layer
        int layerIndex1 = 0, layerIndex2 = 0;
        for (var i = 0; i < SortingLayer.layers.Length; i++)
        {
            var id = SortingLayer.layers[i].id;
            if (id == sr1.sortingLayerID)
            {
                layerIndex1 = i;
            }
            if (id == sr2.sortingLayerID)
            {
                layerIndex2 = i;
            }
        }
        if (layerIndex1 != layerIndex2)
        {
            return layerIndex1 > layerIndex2;
        }
        // then by order in layer
        if (sr1.sortingOrder != sr2.sortingOrder)
        {
            return sr1.sortingOrder > sr2.sortingOrder;
        }
        // render queue and gamera are not implemented here
        // custom axis sort - we use y
        var y1 = sr1.transform.position.y;
        var y2 = sr2.transform.position.y;
        if (y1 != y2)
        {
            return y1 < y2;
        }
        // other criteria are ignored
        return true;
    }

    private void Update()
    {
        // check whether some SpriteOverlap covers us
        var newCovered = false;
        List<SpriteOverlap> objectsToRemove = null;
        foreach (var trackedGameObject in trackedGameObjects)
        {
            if (trackedGameObject == null)
            {
                if (objectsToRemove == null)
                {
                    objectsToRemove = new();
                }
                objectsToRemove.Add(trackedGameObject);
                continue;
            }
            if (trackedGameObject.priority > priority &&
                IsOver(SpriteRenderer, trackedGameObject.SpriteRenderer) &&
                trackedGameObject.Collider.IsTouching(Collider))
            {
                newCovered = true;
                break;
            }
        }
        if (objectsToRemove != null)
        {
            foreach (var objectToRemove in objectsToRemove)
            {
                trackedGameObjects.Remove(objectToRemove);
            }
        }
        // start animation if we changed state
        if (!covered && newCovered)
        {
            StartAlphaAnimation(AlphaWhenSecondary);
        }
        if (covered && !newCovered)
        {
            StartAlphaAnimation(1);
        }
        // save new state
        covered = newCovered;
    }

    private Coroutine colorAnimationCoroutine = null;

    private void StartAlphaAnimation(float alpha)
    {
        if (colorAnimationCoroutine != null)
        {
            StopCoroutine(colorAnimationCoroutine);
        }
        var color = SpriteRenderer.color;
        var newColor = new Color(color.r, color.g, color.b, alpha);
        colorAnimationCoroutine = StartCoroutine(AnimateColor(color, newColor));
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