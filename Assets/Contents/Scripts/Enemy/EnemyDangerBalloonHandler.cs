using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDangerBalloonHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void OnDangerLevelChanged(float delta)
    {
        // foreach(var balloon in Balloons)
        // {
        //     balloon.SetActive(delta > 0);
        // }
        // SpriteMask.alphaCutoff = 1 - delta;
        Debug.Log($"danger delta is {delta}");
        spriteRenderer.enabled = delta > 0;
        animator.Play("warning", 0, delta);
    }
}
