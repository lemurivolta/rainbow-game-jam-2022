using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManifestoManager : MonoBehaviour
{
    public List<Bark> Barks;


    [SerializeField] TMP_Text label;
    [SerializeField] Toggle playerOneSkip;
    [SerializeField] Toggle playerTwoSkip;
    [SerializeField] float timeBetweenLetters = 0.05f;
    [SerializeField] float waitAfterTextIsShowed = 3f;
    [SerializeField] SceneHandler sceneHandler;
    Bark currentBark;

    private void Start()
    {
        BarksLinker(Barks);
        Barks[Barks.Count - 1].nextSceneOnSkip = true;
        PlayBark(Barks[0]);
    }

    public void PlayBark(Bark b)
    {
        currentBark = b;
        currentBark.canSkip = false;

        playerOneSkip.isOn = false;
        playerTwoSkip.isOn = false;

        if (currentBark.pressToSkip)
        {
            //Time.timeScale = 0;
            playerOneSkip.gameObject.SetActive(true);
            playerTwoSkip.gameObject.SetActive(true);
        }
        else
        {
            playerOneSkip.gameObject.SetActive(false);
            playerTwoSkip.gameObject.SetActive(false);
        }

        SetText(currentBark.GetBark());
    }

    public void SetText(string s)
    {
        label.text = "";
        //StopCoroutine(ShowText(s));
        StartCoroutine(ShowText(s));
    }

    IEnumerator ShowText(string s)
    {
        foreach (char c in s)
        {
            label.text += c;
            yield return new WaitForSeconds(timeBetweenLetters);
        }

        yield return new WaitForSeconds(waitAfterTextIsShowed);

        if (!currentBark.pressToSkip)
            OnTextAllShowed();
    }

    private void OnTextAllShowed()
    {
        if (currentBark.pressToSkip)
        {
            playerOneSkip.SetIsOnWithoutNotify(false);
            playerTwoSkip.SetIsOnWithoutNotify(false);
        }

        if (currentBark.nextBark != null)
        {
            PlayBark(currentBark.nextBark);
            return;
        }

        if (currentBark.nextSceneOnSkip)
            sceneHandler.NextScene();
    }

    public void TrySkipping(bool b)
    {
        if (currentBark.pressToSkip && b && playerOneSkip.isOn && playerTwoSkip.isOn)
        {
            currentBark.canSkip = true;

            StopAllCoroutines();

            label.text = currentBark.GetBark();

            StartCoroutine(WaitToSkip());
        }
    }

    IEnumerator WaitToSkip()
    {
        yield return new WaitForSeconds(0.5f);

        playerOneSkip.SetIsOnWithoutNotify(false);
        playerTwoSkip.SetIsOnWithoutNotify(false);

        yield return new WaitForEndOfFrame();

        OnTextAllShowed();
    }


    void BarksLinker(List<Bark> B)
    {
        for (int i = 0; i < B.Count; i++)
        {
            if (i < B.Count - 1)
            {
                B[i].nextBark = B[i + 1];
            }
        }
    }
}
