using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Schermata : MonoBehaviour
{
    [SerializeField] string schermataName;
    [SerializeField] Transform playerOneSpawnPoint;
    [SerializeField] Transform playerTwoSpawnPoint;

    [SerializeField] Barks barks;
}
