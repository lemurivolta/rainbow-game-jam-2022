using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonOnOff : MonoBehaviour
{
    [Tooltip("Duration that the button stays on before turning off. If zero, it never turns off.")]
    [Min(0f)]
    public float Delay = 2f;

    private Animator Animator;

    public UnityEvent DelayExpired;

    private void Start()
    {
        Animator = GetComponent<Animator>();
    }

    private bool IsOn
    {
        get { return Animator.GetBool("On"); }
        set { Animator.SetBool("On", value); }
    }

    public void TurnOn()
    {
        if(!IsOn)
        {
            IsOn = true;
            if (Delay > 0)
            {
                StartCoroutine(OffTimeout());
            }
        }
    }

    private IEnumerator OffTimeout()
    {
        yield return new WaitForSeconds(Delay);
        IsOn = false;
        DelayExpired.Invoke();
    }
}
