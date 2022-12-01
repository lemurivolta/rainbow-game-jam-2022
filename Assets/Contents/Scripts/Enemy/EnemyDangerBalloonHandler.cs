using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDangerBalloonHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void OnDangerLevelChanged(float delta)
    {
        spriteRenderer.enabled = delta > 0;
        animator.Play("warning", 0, delta);
    }
}
