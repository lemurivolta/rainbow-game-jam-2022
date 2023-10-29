using System.Collections;

using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// Handles the game over zoom and camera marking
/// </summary>
public class GameOverManagement : MonoBehaviour
{
    private static GameOverManagement instance;

    public static GameOverManagement Instance => instance;

    private bool inGameOverSequence;

    public static bool InGameOverSequence => instance && instance.inGameOverSequence;

    [SerializeField] private float zoomInDuration = 0.5f;
    [SerializeField] private float zoomWaitDuration = 0.5f;
    [SerializeField] private float zoomOutDuration = 0.5f;
    //[SerializeField] private float zoomFactor = 1.1f;

    private Rect? rect;

    public static IEnumerator StartZoomIn(GameOverCauseSpriteRenderer cause)
    {
        instance.inGameOverSequence = true;

        var mainCamera = Camera.main;
        var ppcamera = mainCamera.GetComponent<PixelPerfectCamera>();
        ppcamera.enabled = false;

        var sourcePosition = mainCamera.transform.position;
        var sourceOrthographicSize = mainCamera.orthographicSize;

        var rect = cause.GetBoundaryWorldRect();
        instance.rect = rect;

        try
        {
            Vector3 destPosition = rect.center;
            destPosition.z = sourcePosition.z;
            var destOrthographicSize = rect.height; // double height, and then halved because that's how orthographic size works

            // zoom in
            var startTime = Time.time;
            var endTime = startTime + instance.zoomInDuration;
            while (Time.time < endTime)
            {
                var d = (Time.time - startTime) / instance.zoomInDuration;
                mainCamera.transform.position = Vector3.Lerp(sourcePosition, destPosition, d);
                mainCamera.orthographicSize = Mathf.Lerp(sourceOrthographicSize, destOrthographicSize, d);
                Debug.Log($"from {sourceOrthographicSize} to {destOrthographicSize} at {d} is {mainCamera.orthographicSize}");
                yield return null;
            }

            // stay
            yield return new WaitForSeconds(instance.zoomWaitDuration);

            // zoom out
            startTime = Time.time;
            endTime = startTime + instance.zoomOutDuration;
            while (Time.time < endTime)
            {
                var d = (Time.time - startTime) / instance.zoomOutDuration;
                mainCamera.transform.position = Vector3.Lerp(destPosition, sourcePosition, d);
                mainCamera.orthographicSize = Mathf.Lerp(destOrthographicSize, sourceOrthographicSize, d);
                yield return null;
            }
        }
        finally
        {
            instance.inGameOverSequence = false;
            mainCamera.transform.position = sourcePosition;
            mainCamera.orthographicSize = sourceOrthographicSize;
            ppcamera.enabled = true;
            instance.rect = null;
        }
    }

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void Update()
    //{
    //    if (instance.rect.HasValue)
    //    {
    //        Debug.Log(instance.rect.Value);
    //        Vector2 min = instance.rect.Value.min;
    //        Vector2 max = instance.rect.Value.max;
    //        var a = new Vector2(max.x, min.y);
    //        var b = new Vector2(min.x, max.y);
    //        Debug.DrawLine(min, a, Color.red, 0, false);
    //        Debug.DrawLine(a, max, Color.red, 0, false);
    //        Debug.DrawLine(max, b, Color.red, 0, false);
    //        Debug.DrawLine(b, min, Color.red, 0, false);
    //    }
    //}
}
