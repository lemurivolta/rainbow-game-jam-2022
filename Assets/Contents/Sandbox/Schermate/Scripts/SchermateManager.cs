using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchermateManager : Singleton<SchermateManager>
{
    [SerializeField] List<Schermata> Schermate;

    public void GoToNext()
    {
        Debug.Log("GoToNext()");
    }

    public void Restart()
    {
        Debug.Log("restart()");
    }
}
