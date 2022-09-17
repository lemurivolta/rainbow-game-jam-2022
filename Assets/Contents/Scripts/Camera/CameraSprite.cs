using UnityEngine;

public class CameraSprite : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;

    private float Angle;

    public Sprite[] Sprites;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        UpdateAngle();
    }

    public void OnAngleChanged(float x)
    {
        Angle = x;
        UpdateAngle();
    }

    private void UpdateAngle()
    {
        if (SpriteRenderer == null)
        {
            return;
        }
        var i = Mathf.Clamp(
            (int)Mathf.Floor(Sprites.Length * (Angle + 1) / 2f),
            0,
            Sprites.Length - 1
        );
        SpriteRenderer.sprite = Sprites[i];
    }
}
