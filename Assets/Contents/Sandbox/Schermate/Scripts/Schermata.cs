using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Schermata : MonoBehaviour
{
    [SerializeField] string schermataName;
    [SerializeField] public Transform playerOneSpawnPoint;
    [SerializeField] public Transform playerTwoSpawnPoint;

    [SerializeField] public SchermataBarks barks;
    [SerializeField] public AudioClip music;
}
