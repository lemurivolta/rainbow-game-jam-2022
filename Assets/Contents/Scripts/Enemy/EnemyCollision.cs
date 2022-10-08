using UnityEngine;
using UnityEngine.Events;

public class EnemyCollision : MonoBehaviour
{
    public float BaseDanger = 0f;

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
        StartDanger = BaseDanger;
        DangerLevelChanged.Invoke(StartDanger);
    }

    public void OnGotVisible()
    {
        if (NumCharactersInside == 0)
        {
            SetupNewDangerSlope(1 / FullDangerTime);
        }
        NumCharactersInside++;
    }

    public void OnGotHidden()
    {
        NumCharactersInside--;
        if (NumCharactersInside == 0)
        {
            SetupNewDangerSlope(-1 / FullDangerTime);
        }
    }

    private void SetupNewDangerSlope(float dangerPerSecond)
    {
        var currentDanger = GetCurrentDangerValue();
        StartTime = Time.time;
        DangerPerSecond = dangerPerSecond;
        StartDanger = currentDanger;
    }

    private float GetCurrentDangerValue()
    {
        return Mathf.Clamp01(StartDanger + (Time.time - StartTime) * DangerPerSecond);
    }

    private void Update()
    {
        var dangerValue = GetCurrentDangerValue();
        if (LastDangerValue != dangerValue)
        {
            DangerLevelChanged.Invoke(dangerValue);
            if (dangerValue >= 1)
            {
                 SchermateManager.Instance.GameOver();
            }
            else if (dangerValue <= BaseDanger)
            {
                StartDanger = BaseDanger;
                StartTime = Time.time;
                DangerPerSecond = 0;
            }
            LastDangerValue = dangerValue;
        }
    }
}
