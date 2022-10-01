using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Balloon : Singleton<Balloon>
{
    [SerializeField] TMP_Text label;
    [SerializeField] RectTransform image;
    [SerializeField] Toggle playerOneSkip;
    [SerializeField] Toggle playerTwoSkip;
    [SerializeField] float timeBetweenLetters = 0.05f;
    [SerializeField] float yOffset = 1.4f;
    [SerializeField] float waitAfterTextIsShowed = 3f;
    [SerializeField] GameObject mainPanel;
    [SerializeField] AudioSource audioSource;
    [HideInInspector] public bool isBarking;
    Bark currentBark;


    /*
    private void Start()
    {
        SetText("Bla bla bla bla! Bla bla bla, bla bla. Bla, bla bla... bla bla bla bla.");
    }
    */

    private void Update()
    {
        if (isBarking)
            PlaceBalloon();
    }

    public void PlayBark(Bark b)
    {
        isBarking = true;

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

        PlaceBalloon();
        mainPanel.SetActive(true);

        if (currentBark.soundEffect != null)
        {
            audioSource.clip = currentBark.soundEffect;
            audioSource.Stop();
            audioSource.Play();
        }

        SetText(currentBark.GetBark());
    }

    [SerializeField] private float maxY = 5.7f;

    private void PlaceBalloon()
    {
        if (currentBark == null)
            return;

        if (currentBark.character == CHARACTER.NPC)
        {
            if (currentBark.targetTransform != null)
            {
                var veryHigh = currentBark.targetTransform.position.y > maxY;
                label.transform.localScale = image.transform.localScale = new Vector3(1, veryHigh ? -1 : 1, 1);
                transform.position = currentBark.targetTransform.position + (veryHigh ? Vector3.down : Vector3.up) * yOffset;
            }
            else
            {
                transform.position = Vector3.zero;
            }
        }
        else
        {
            CharacterInfo ci = CharacterInfo.AllCharacterControlledBy.Find((x) => x.name.ToUpper() == currentBark.character.ToString());
            if (ci)
                transform.position = ci.transform.position + Vector3.up * yOffset;
            else
                Debug.LogError("Character not found in scene: " + currentBark.character.ToString());
        }
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
        audioSource.Stop();
        mainPanel.SetActive(false);

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

        isBarking = false;

        if (currentBark.restartAtEnd)
        {
            SchermateManager.Instance.Restart();
            return;
        }

        if (currentBark.nextSceneOnSkip)
            SchermateManager.Instance.GoToNext();
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

    /*todo
     * gestire lo scorrimento del testo
     * gestire l'andare avanti se tutte le giocatrici hanno cliccato il tasto azione
    */
}
