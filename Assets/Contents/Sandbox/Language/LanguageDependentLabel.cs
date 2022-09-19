using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageDependentLabel : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    [SerializeField] string line_ita, line_eng;

    private void Awake()
    {
        if (label == null)
            label = GetComponent<TMP_Text>();        
    }

    private void OnEnable()
    {
        SetText();
        LanguageManager.OnChangeLanguage += SetText;
    }

    private void OnDisable()
    {
        LanguageManager.OnChangeLanguage -= SetText;
    }

    private void SetText()
    {
        switch (LanguageManager.GetLanguage())
        {
            case Language.ITA:
                label.text = line_ita;
                break;
            case Language.ENG:
                label.text = line_eng;
                break;
            default:
                label.text = line_ita;
                break;
        }
    }
}
