using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barks : MonoBehaviour
{
    [SerializeField] List<Bark> startingBarks;
    [SerializeField] List<Bark> middlePointBarks;
    [SerializeField] List<Bark> endingBarks;

    int maximumCharShowed = 70;

    int charIndex;


    public void PlayBarksList(List<Bark> Barks)
    {
        // metti in pausa la scena
        // mostra 
    }

    public void PlayBark(Bark bark)
    {
        // aggiorna la posizione del baloon 
        // aggiorna il testo
    }
}
