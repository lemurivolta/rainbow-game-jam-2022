using System.Collections.Generic;

using UnityEngine;

public class CameraStateUpdater : MonoBehaviour
{
    public LayerMask CameraLayer;

    public State State;

    public int NumCameras => cameras.Count;

    private readonly HashSet<GameObject> cameras = new();

    public IEnumerable<GameObject> Cameras => cameras;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((CameraLayer & (1 << collision.gameObject.layer)) != 0)
        {
            cameras.Add(collision.gameObject);
            UpdateState();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((CameraLayer & (1 << collision.gameObject.layer)) != 0)
        {
            cameras.Remove(collision.gameObject);
            UpdateState();
        }
    }

    private void UpdateState()
    {
        State.Set(NumCameras > 0);
    }
}
