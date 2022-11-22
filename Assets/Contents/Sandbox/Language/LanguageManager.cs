using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language { ITA, ENG }

public static class LanguageManager
{
    public static Language currentLanguage = Language.ITA;
    public delegate void ChangeLanguage();
    public static event ChangeLanguage OnChangeLanguage;

    public static Language GetLanguage()
    {
        return currentLanguage;
    }

    public static void SetLanguage(int l)
    {
        try
        {
            currentLanguage = (Language)l;
            Debug.Log("New language: " + currentLanguage.ToString());
            if (OnChangeLanguage != null)
                OnChangeLanguage();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
            throw;
        }

    }

    public static void SetLanguage(Language lang)
    {
        currentLanguage = lang;
        Debug.Log("New language: " + currentLanguage.ToString());
    }
}
