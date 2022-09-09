using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageHandler : MonoBehaviour
{
    public void SetLanguage(Language lang)
    {
        LanguageManager.SetLanguage(lang);        
    }

    public void SetLanguage(int lang)
    {
        LanguageManager.SetLanguage(lang);
    }
}
