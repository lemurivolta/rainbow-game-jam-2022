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

    public GameObject GetCompanion()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach(var gameObject in gameObjects)
        {
            if(gameObject.GetComponent<CharacterControlledBy>().Player == this.Player &&
                gameObject != this.gameObject)
            {
                return gameObject;
            }
        }
        throw new System.Exception("Cannot find companion of " + gameObject.name);
    }
}
