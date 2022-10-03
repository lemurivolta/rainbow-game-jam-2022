using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class SchermateManager : Singleton<SchermateManager>
{
    [SerializeField] List<GameObject> PrefabSchermate;
    [SerializeField] SceneHandler sceneHandler;
    AudioSource audioSource;

    int currentSchermataIndex = 0;
    GameObject currentSchermata;
    bool allowedToSkip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        FirstStart();
    }

#if UNITY_EDITOR
    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Space))
            EndSchermata();
        if (Input.GetKeyDown(KeyCode.R))
            Restart();
        if (Input.GetKeyDown(KeyCode.G))
            GameOver();
    }
#endif

    public void EndSchermata()
    {
//        Balloon.Instance.StopAllCoroutines();
        currentSchermata.GetComponent<Schermata>().barks.PlayEndingBarks();
    }

    public void GoToNext()
    {

        Destroy(currentSchermata);

        currentSchermataIndex++;

        if (currentSchermataIndex <= PrefabSchermate.Count - 1)
            FirstStart();
        else if (sceneHandler.IsThereASceneAfterThis())
            sceneHandler.NextScene();
        else
            sceneHandler.GoTo();
    }

    public void FirstStart()
    {
        currentSchermata = Instantiate(PrefabSchermate[currentSchermataIndex]);
        //PlaceCharacters();
        currentSchermata.GetComponent<Schermata>().barks.PlayStartingBarks();
        audioSource.clip = currentSchermata.GetComponent<Schermata>().music;
        audioSource.Play();
    }

    public void GameOver()
    {
        Debug.Log("GameOver()");
        Balloon.Instance.StopAllCoroutines();
        currentSchermata.GetComponent<Schermata>().barks.PlayGameOverBark();
    }
    public void Restart()
    {
        Debug.Log("Restart()");
        // fade

        Balloon.Instance.StopAllCoroutines();

        // distruggi schermata
        Destroy(currentSchermata);
        // reistanzia schermata
        currentSchermata = Instantiate(PrefabSchermate[currentSchermataIndex]);

        
    }
}
