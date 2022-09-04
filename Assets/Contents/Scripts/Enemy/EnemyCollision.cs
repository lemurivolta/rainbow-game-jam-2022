using UnityEngine;
using UnityEngine.Events;

public class EnemyCollision : MonoBehaviour
{
    public float FullDangerTime = 1f;

    public UnityEvent<float> DangerLevelChanged;

    private int NumCharactersInside = 0;

    private float StartTime = -1;

    private float StartDanger = 0;

    private float DangerPerSecond = 0;

    private float LastDangerValue = 0;

    private void Start()
    {
        StartTime = Time.time;
        DangerPerSecond = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("On trigger enter");
        if (NumCharactersInside == 0)
        {
            SetupNewDangerSlope(1 / FullDangerTime);
        }
        NumCharactersInside++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        NumCharactersInside--;
        if (NumCharactersInside == 0)
        {
            SetupNewDangerSlope(-1 / FullDangerTime);
        }
    }

    private void SetupNewDangerSlope(float dangerPerSecond)
    {
        Debug.Log($"Setup new slope: {dangerPerSecond}");
        var currentDanger = GetCurrentDangerValue();
        StartTime = Time.time;
        DangerPerSecond = dangerPerSecond;
        StartDanger = currentDanger;
    }

    private float GetCurrentDangerValue()
    {
        return StartDanger + (Time.time - StartTime) * DangerPerSecond;
    }

    private void Update()
    {
        var dangerValue = GetCurrentDangerValue();
        if (LastDangerValue != dangerValue)
        {
            DangerLevelChanged.Invoke(dangerValue);
            if (dangerValue >= 1)
            {
                SchermateManager.Instance.Restart();
            }
            else if (dangerValue <= 0)
            {
                StartDanger = 0;
                StartTime = Time.time;
                DangerPerSecond = 0;
            }
            LastDangerValue = dangerValue;
        }
    }
}
