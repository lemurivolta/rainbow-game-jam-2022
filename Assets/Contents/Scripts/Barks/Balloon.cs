using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Assertions;

public class Balloon : Singleton<Balloon>
{
    [System.Serializable]
    public struct UIReferences
    {
        public TMP_Text label;
        public RectTransform image;
        public Toggle playerOneSkip;
        public Toggle playerTwoSkip;
    }
    [SerializeField] UIReferences TopBalloon;
    [SerializeField] UIReferences BottomBalloon;
    [SerializeField] UIReferences LeftBalloon;
    [SerializeField] UIReferences RightBalloon;
    [SerializeField] float timeBetweenLetters = 0.05f;
    [SerializeField] float yOffset = 0;
    [SerializeField] float xOffset = 0;
    [SerializeField] float waitAfterTextIsShown = 3f;
    [SerializeField] GameObject mainPanel;
    [SerializeField] AudioSource audioSource;
    [HideInInspector] public bool isBarking;
    Bark currentBark;

    [SerializeField] GameObject resetButton;

    [SerializeField] private Camera mainCamera;

    private CanvasScaler canvasScaler;

    /*
    private void Start()
    {
        SetText("Bla bla bla bla! Bla bla bla, bla bla. Bla, bla bla... bla bla bla bla.");
    }
    */

    private void Start()
    {
        canvasScaler = GetComponentInParent<CanvasScaler>();
    }

    private void Update()
    {
        if (isBarking)
            PlaceBalloon();
    }

    private void OnAllBalloons(Action<UIReferences> balloonAction)
    {
        balloonAction(TopBalloon);
        balloonAction(BottomBalloon);
        balloonAction(LeftBalloon);
        balloonAction(RightBalloon);
    }

    private void OnAllPlayerOneSkips(Action<Toggle> toggleAction)
    {
        OnAllBalloons(uiReferences => toggleAction(uiReferences.playerOneSkip));
    }

    private void OnAllPlayerTwoSkips(Action<Toggle> toggleAction)
    {
        OnAllBalloons(uiReferences => toggleAction(uiReferences.playerTwoSkip));
    }

    private void OnAllPlayerSkips(Action<Toggle> toggleAction)
    {
        OnAllPlayerOneSkips(toggleAction);
        OnAllPlayerTwoSkips(toggleAction);
    }

    private bool ForAllPlayerSkips(Predicate<Toggle> predicate)
    {
        bool ok = true;
        OnAllPlayerSkips(playerSkip => ok = ok && predicate(playerSkip));
        return ok;
    }

    private void OnAllLabels(Action<TMP_Text> labelAction)
    {
        OnAllBalloons(uiReferences => labelAction(uiReferences.label));
    }

    private void OnAllImages(Action<RectTransform> imageAction)
    {
        OnAllBalloons(uiReferences => imageAction(uiReferences.image));
    }


    public void PlayBark(Bark b)
    {
        isBarking = true;
        resetButton.SetActive(false);

        currentBark = b;
        currentBark.canSkip = false;

        OnAllPlayerSkips(playerSkip =>
        {
            playerSkip.isOn = false;
            playerSkip.gameObject.SetActive(currentBark.pressToSkip);
        });

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

        Vector3 position = Vector3.zero;
        if (currentBark.character == CHARACTER.NPC)
        {
            if (currentBark.targetTransform != null)
            {
                position = currentBark.targetTransform.position;
            }
            else
            {
                Debug.LogWarning($"Could not find a target for bark '{currentBark.line_eng}' / '{currentBark.line_ita}'");
            }
        }
        else
        {
            CharacterInfo ci = CharacterInfo.AllCharacterInfos
                .Find((x) => x.Character == currentBark.character);
            Assert.IsNotNull(ci);
            position = ci.transform.position;
        }

        var sf = canvasScaler.scaleFactor;
        var balloonHeight = TopBalloon.image.sizeDelta.y * sf; // top is the default balloon if possible
        var balloonWidth = TopBalloon.image.sizeDelta.x * sf;
        var screenPoint = (Vector2)mainCamera.WorldToScreenPoint(position);
        int chosenBalloon; // 0 = top, 1 = bottom, 2 = left, 3 = right
        var tooHigh = screenPoint.y + yOffset + balloonHeight > Screen.height;
        bool tooRight = screenPoint.x + balloonWidth / 2 > Screen.width;
        bool tooLeft = screenPoint.x - balloonWidth / 2 < 0;
        if (tooHigh && !tooRight)
        {
            // we're too high: use the bottom balloon
            chosenBalloon = 1;
            BottomBalloon.image.anchoredPosition = screenPoint / sf + Vector2.down * yOffset;
        }
        else if (tooRight)
        {
            // we're too much to the right: use left balloon
            chosenBalloon = 2;
            LeftBalloon.image.anchoredPosition = screenPoint / sf + Vector2.left * xOffset;
        }
        else if (tooLeft)
        {
            // we're too much to the left: use right balloon
            chosenBalloon = 3;
            RightBalloon.image.anchoredPosition = screenPoint / sf + Vector2.right * xOffset;
        }
        else
        {
            // top balloon will fit, and this is the default
            chosenBalloon = 0;
            TopBalloon.image.anchoredPosition = screenPoint / sf + Vector2.up * yOffset;
        }
        TopBalloon.image.gameObject.SetActive(chosenBalloon == 0);
        BottomBalloon.image.gameObject.SetActive(chosenBalloon == 1);
        LeftBalloon.image.gameObject.SetActive(chosenBalloon == 2);
        RightBalloon.image.gameObject.SetActive(chosenBalloon == 3);
    }

    public void SetText(string s)
    {
        OnAllLabels(label => label.text = "");
        //StopCoroutine(ShowText(s));
        StartCoroutine(ShowText(s));
    }

    IEnumerator ShowText(string s)
    {
        foreach (char c in s)
        {
            OnAllLabels(label => label.text += c);
            yield return new WaitForSeconds(timeBetweenLetters);
        }

        yield return new WaitForSeconds(waitAfterTextIsShown);

        if (!currentBark.pressToSkip)
        {
            OnTextAllShown();
        }
    }

    private void OnTextAllShown()
    {
        audioSource.Stop();
        mainPanel.SetActive(false);

        if (currentBark.pressToSkip)
        {
            OnAllPlayerSkips(playerSkip => playerSkip.SetIsOnWithoutNotify(false));
        }

        if (currentBark.nextBark != null)
        {
            PlayBark(currentBark.nextBark);
            return;
        }

        isBarking = false;
        resetButton.SetActive(true);

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
        if (isBarking && currentBark.pressToSkip && b && ForAllPlayerSkips(playerSkip => playerSkip.isOn))
        {
            currentBark.canSkip = true;

            StopAllCoroutines();

            OnAllLabels(label => label.text = currentBark.GetBark());

            StartCoroutine(WaitToSkip());
        }
    }

    IEnumerator WaitToSkip()
    {
        yield return new WaitForSeconds(0.5f);

        OnAllPlayerSkips(playerSkip => playerSkip.SetIsOnWithoutNotify(false));

        yield return new WaitForEndOfFrame();

        OnTextAllShown();
    }

    /*todo
     * gestire lo scorrimento del testo
     * gestire l'andare avanti se tutte le giocatrici hanno cliccato il tasto azione
    */
}
