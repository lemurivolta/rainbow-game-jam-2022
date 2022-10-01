using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchermataBarks : MonoBehaviour
{
    [SerializeField] List<Bark> startingBarks;
    [SerializeField] List<Bark> gameOverBarks;
    [SerializeField] List<Bark> endingBarks;

    int maximumCharShowed = 70;

    int charIndex;

    private void Awake()
    {
        BarksLinker(startingBarks);
        BarksLinker(endingBarks);
        foreach (Bark b in gameOverBarks)
        {
            b.restartAtEnd = true;
        }
        if(endingBarks.Count > 0)
           endingBarks[endingBarks.Count - 1].nextSceneOnSkip = true;
    }

    void BarksLinker(List <Bark> B)
    {
        for (int i = 0; i < B.Count; i++)
        {
            if(i < B.Count-1)
            {
                B[i].nextBark = B[i + 1];
            }
        }    
    }

    public void PlayStartingBarks()
    {
            Balloon.Instance.PlayBark(startingBarks[0]);
    }

    public void PlayEndingBarks()
    {
        Balloon.Instance.PlayBark(endingBarks[0]);
    }

    public void PlayGameOverBark()
    {
        Balloon.Instance.PlayBark(gameOverBarks[UnityEngine.Random.Range(0, gameOverBarks.Count-1)]);
    }
}
