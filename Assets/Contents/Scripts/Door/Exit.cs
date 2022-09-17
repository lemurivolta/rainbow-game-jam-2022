using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public bool IsExit = false;

    public bool AutomaticallyOpen = false;

    public bool CanExit { get; set; }

    public Actionable OpenActionable;

    private void Start()
    {
        if (IsExit && AutomaticallyOpen)
        {
            OpenActionable.PerformAction();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log($"In trigger! IsExit = {IsExit}, CanExit = {CanExit}, collision.gameObject = {collision.gameObject}, isci = {collision.gameObject.transform.parent.GetComponent<CharacterInfo>() != null}");
        // check if a character entered the exit zone
        if (IsExit &&
            CanExit &&
            collision.gameObject.transform
            .parent.GetComponent<CharacterInfo>() != null)
        {
            // Debug.Log("Go to next level");
            SchermateManager.Instance.EndSchermata();
        }
    }
}
