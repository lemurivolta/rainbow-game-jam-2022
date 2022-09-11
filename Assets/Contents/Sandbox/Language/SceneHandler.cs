using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void NextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PrevScene()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }

    public bool IsThereASceneAfterThis()
    {
        return SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1;
    }

    public void GoTo(int i = 0)
    {
        SceneManager.LoadScene(i);
    }
}
