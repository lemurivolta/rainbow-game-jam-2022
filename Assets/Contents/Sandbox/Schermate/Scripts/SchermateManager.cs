using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchermateManager : Singleton<SchermateManager>
{
    [SerializeField] List<Schermata> Schermate;

    int currentSchermata = 0;

    private void Start()
    {
        Restart();
    }

    public void GoToNext()
    {
        Debug.Log("GoToNext()");
        // fai partire eventuali bark di fine schermata
        Schermate[currentSchermata].gameObject.SetActive(false);
        currentSchermata++;
        
        Restart();
    }

    public void Restart()
    {
        Debug.Log("restart()");
        // posiziona le personagge
        // fai partire eventuali bark
        // azzera interruttori e npc
        Schermate[currentSchermata].gameObject.SetActive(true);
    }
}
