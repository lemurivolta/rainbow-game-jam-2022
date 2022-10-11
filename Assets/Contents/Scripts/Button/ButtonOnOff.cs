using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that handles turning the button on and off.
/// </summary>
public class ButtonOnOff : MonoBehaviour
{
    [Tooltip("Duration that the button stays on before turning off. If zero, it never turns off.")]
    [Min(0f)]
    public float Delay = 2f;

    /// <summary>
    /// Animator to change the currently used animation.
    /// </summary>
    private Animator Animator;

    /// <summary>
    /// Event invoked whenever the button is pressed. The duration is passed along.
    /// </summary>
    public UnityEvent<float> ButtonPressed;

    /// <summary>
    /// Event invoked whenever the button is reset.
    /// </summary>
    public UnityEvent DelayExpired;

    private void Start()
    {
        Animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Whether the button is on or off.
    /// </summary>
    private bool IsOn
    {
        get { return Animator.GetBool("On"); }
        set { Animator.SetBool("On", value); }
    }

    /// <summary>
    /// Callback invoked whenever the button is interacted with.
    /// </summary>
    public void TurnOn()
    {
        if (!IsOn)
        {
            IsOn = true;
            if (Delay > 0)
            {
                StartCoroutine(OffTimeout());
            }
            ButtonPressed.Invoke(Delay);
        }
    }

    /// <summary>
    /// Coroutine used to turn off the button after a certain amount of time has passed.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OffTimeout()
    {
        yield return new WaitForSeconds(Delay);
        if (!Balloon.Instance.isBarking)
        {
            IsOn = false;
            DelayExpired.Invoke();
        }
    }
}
