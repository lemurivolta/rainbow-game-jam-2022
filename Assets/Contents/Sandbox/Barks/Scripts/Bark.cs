public enum CHARACTER { NPC, MARCELLA, MARIELLE, SARAH, YELENA }
[System.Serializable]
public class Bark
{
    public CHARACTER character;
    public string line_ita;
    public string line_eng;

    public void Play()
    {
        Balloon.Instance.PlayBark(this);
    }

    public string GetBark()
    {
        return line_ita;
    }
}