using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class NPCBarks : MonoBehaviour
{
    [SerializeField] List<Bark> Barks;
    [Tooltip("Se desideri un tempo fisso, ti basta dare a min e max lo stesso valore.")]
    [SerializeField] float minTimeToBark = 3;
    [SerializeField] float maxTimeToBark = 6;
    [SerializeField] float barkLifetime = 5;
    [SerializeField] TMP_Text balloonText;
    [SerializeField] GameObject balloonGO;

    float timer;
    bool isShowingBark = false;

    private void Start()
    {
        balloonGO.SetActive(false);
        ResetTimer();
    }

    private void Update()
    {
        if (isShowingBark)
            return;

        UpdateTimer();        
        CheckTimer();
    }

    void ResetTimer()
    {
        timer = Random.Range(minTimeToBark, maxTimeToBark);        
    }


    void CheckTimer()
    {
        if (timer <= 0)
            TimeIsOut();
    }

    private void TimeIsOut()
    {
        StartCoroutine(ShowBalloon());        
    }

    IEnumerator ShowBalloon()
    {
        isShowingBark = true;
        balloonGO.SetActive(true);
        int randomIndex = Random.Range(0, Barks.Count);
        balloonText.text = Barks[randomIndex].GetBark();

        yield return new WaitForSeconds(barkLifetime);
        isShowingBark = false;
        balloonGO.SetActive(false);
        ResetTimer();
    }

    void UpdateTimer()
    {
        timer -= Time.deltaTime;        
    }
}
