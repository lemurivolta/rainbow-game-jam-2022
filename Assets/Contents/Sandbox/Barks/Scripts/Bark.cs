using UnityEngine;

public enum CHARACTER { NPC, CLOE, MARIELLE, SARAH, YELENA }
[System.Serializable]
public class Bark
{
    public CHARACTER character;
    public string line_ita;
    public string line_eng;
    [HideInInspector] public bool canSkip = false;
    public Transform targetTransform;
    public AudioClip soundEffect;
    public Bark nextBark { get; set; }
    public bool pressToSkip = true;
    [HideInInspector] public bool nextSceneOnSkip;

    [HideInInspector] public bool restartAtEnd = false;

    


    public void Play()
    {
        Balloon.Instance.PlayBark(this);
    }

    public string GetBark()
    {
        switch (LanguageManager.GetLanguage())
        {
            case Language.ITA:
                return line_ita;
                break;
            case Language.ENG:
                return line_eng;
                break;
            default:
                return line_ita;
                break;
        }        
    }
}