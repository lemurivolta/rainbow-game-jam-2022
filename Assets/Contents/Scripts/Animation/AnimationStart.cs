using UnityEngine;
using UnityEngine.Playables;

public class AnimationStart : MonoBehaviour
{
    public GameObject Root;

    private PlayableDirector PlayableDirector;

    // Start is called before the first frame update
    void Start()
    {
        PlayableDirector = Root.GetComponentInChildren<PlayableDirector>();
    }

    public void Play()
    {
        PlayableDirector.Play();
    }
}
