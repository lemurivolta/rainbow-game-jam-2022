using System.Collections;
using UnityEngine;

/// <summary>
/// Component that commands a timer animation on top of the button as the button ticks towards its
/// reset.
/// </summary>
public class ButtonTimer : MonoBehaviour
{
    /// <summary>
    /// Animator that contains the timer animation.
    /// </summary>
    [SerializeField] private Animator animator;

    /// <summary>
    /// Renderer that renders the timer animation.
    /// </summary>
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // button can be left- or right- oriented, but timer cannot!
        if (transform.lossyScale.x < 0)
        {
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    /// <summary>
    /// Callback function invoked when the button is pressed.
    /// </summary>
    /// <param name="duration">Duration of the button, or 0 if it stays on.</param>
    public void OnButtonPressed(float duration)
    {
        if (duration > 0)
        {
            StartCoroutine(OnButtonPressedCoroutine(duration));
        }
    }

    /// <summary>
    /// Coroutine that controls the timer animation so that it ends with the
    /// reset of the button.
    /// </summary>
    /// <param name="duration">Duration of the button (> 0)</param>
    /// <returns></returns>
    private IEnumerator OnButtonPressedCoroutine(float duration)
    {
        var startTime = Time.time;
        float delta = 0;

        // sets the timer animation frame at each frame
        spriteRenderer.enabled = true;
        do
        {
            delta = (Time.time - startTime) / duration;
            animator.Play("Timer", 0, delta);
            yield return null;
        } while (delta < 1f);
        spriteRenderer.enabled = false;
    }
}
