using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchermateManager : Singleton<SchermateManager>
{
    [SerializeField] List<Schermata> Schermate;

    int currentSchermata = 0;
    bool allowedToSkip;

    private void Start()
    {
        FirstStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EndSchermata();
        if (Input.GetKeyDown(KeyCode.R))
            Restart();
        if (Input.GetKeyDown(KeyCode.G))
            GameOver();
    }

    public void EndSchermata()
    {
        Schermate[currentSchermata].barks.PlayEndingBarks();
    }

    public void GoToNext()
    {   
        Schermate[currentSchermata].gameObject.SetActive(false);
        currentSchermata++;

        FirstStart();
    }

    public void FirstStart()
    {
        Schermate[currentSchermata].gameObject.SetActive(true);
        PlaceCharacters();
        Schermate[currentSchermata].barks.PlayStartingBarks();
        // fai partire eventuali bark
        // azzera interruttori e npc
    }

    public void GameOver()
    {
        Schermate[currentSchermata].barks.PlayGameOverBark();
        // Restart
    }
    public void Restart()
    {
        Debug.Log("restart()");
        PlaceCharacters();
        // azzera interruttori e npc        
    }


    void PlaceCharacters()
    {
        foreach (var ccb in CharacterInfo.AllCharacterControlledBy)
        {
            if (!ccb.IsFollower)
            {
                if (ccb.Player == CharacterInfo.Players.P1)
                {
                    ccb.transform.position = Schermate[currentSchermata].playerOneSpawnPoint.position;
                }
                else
                {
                    ccb.transform.position = Schermate[currentSchermata].playerTwoSpawnPoint.position;
                }
            }
        }
    }
}
