using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlledBy : MonoBehaviour
{
    public enum Players
    {
        P1,
        P2
    }

    public Players Player;

    private GameObject _Companion = null;

    /// <summary>
    /// The other character controlled by the same player.
    /// </summary>
    public GameObject Companion
    {
        get
        {
            if (_Companion != null) return _Companion;

            var gameObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.GetComponent<CharacterControlledBy>().Player == this.Player &&
                    gameObject != this.gameObject)
                {
                    _Companion = gameObject;
                    return _Companion;
                }
            }
            throw new System.Exception("Cannot find companion of " + gameObject.name);
        }
    }

    public GameObject GetCompanion()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (var gameObject in gameObjects)
        {
            if (gameObject.GetComponent<CharacterControlledBy>().Player == this.Player &&
                gameObject != this.gameObject)
            {
                return gameObject;
            }
        }
        throw new System.Exception("Cannot find companion of " + gameObject.name);
    }
}
