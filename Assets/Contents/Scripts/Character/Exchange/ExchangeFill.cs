using UnityEngine;

public class ExchangeFill : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        SetProgress(0);
    }

    public void OnTimeoutProgress(float newValue)
    {
        SetProgress(newValue);
    }

    private void SetProgress(float newValue)
    {
        animator.Play("Exchange", 0, newValue);
    }
}
