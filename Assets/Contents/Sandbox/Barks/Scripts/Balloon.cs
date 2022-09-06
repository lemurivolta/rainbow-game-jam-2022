using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Balloon : Singleton<Balloon>
{
    [SerializeField] TMP_Text label;
    [SerializeField] Toggle playerOneGo;
    [SerializeField] Toggle playerTwoGo;
    [SerializeField] float timeBetweenLetters = 0.05f;
    
    void SetPosition()
    {

    }

    /*
    private void Start()
    {
        SetText("Bla bla bla bla! Bla bla bla, bla bla. Bla, bla bla... bla bla bla bla.");
    }
    */

    public void PlayBark(Bark b)
    {

    }

    public void SetText(string s)
    {
        Time.timeScale = 0;

        playerOneGo.isOn = false;
        playerTwoGo.isOn = false;

        label.text = "";
        StartCoroutine(ShowText(s));
    }

    IEnumerator ShowText(string s)
    {
        foreach (char c in s)
        {
            label.text += c;
            yield return new WaitForSeconds(timeBetweenLetters);
        }
    }

    /*todo
     * gestire lo scorrimento del testo
     * gestire l'andare avanti se tutte le giocatrici hanno cliccato il tasto azione
    */
}
