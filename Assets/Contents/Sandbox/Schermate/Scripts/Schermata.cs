using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SchermataBarks))]

[System.Serializable]
public class Schermata : MonoBehaviour
{
    [HideInInspector] public SchermataBarks barks;
    [SerializeField] public AudioClip music;

    private void Awake()
    {
        barks = GetComponent<SchermataBarks>();
    }
}
