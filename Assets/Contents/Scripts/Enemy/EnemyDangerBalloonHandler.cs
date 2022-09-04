using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDangerBalloonHandler : MonoBehaviour
{
    public GameObject[] Balloons;
    public SpriteMask SpriteMask;

    public void OnDangerLevelChanged(float delta)
    {
        foreach(var balloon in Balloons)
        {
            balloon.SetActive(delta > 0);
        }
        SpriteMask.alphaCutoff = 1 - delta;
    }
}
