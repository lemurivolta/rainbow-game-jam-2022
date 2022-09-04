using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorActionable : MonoBehaviour
{
    public Animator Animator;

    public bool Opened
    {
        get { return Animator.GetBool("Opened"); }
        set { Animator.SetBool("Opened", value); }
    }
}
