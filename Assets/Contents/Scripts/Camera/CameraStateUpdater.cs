using System.Collections.Generic;
using UnityEngine;

public class CameraStateUpdater : MonoBehaviour
{
    public LayerMask CameraLayer;

    public State State;

    private int _NumCameras = 0;

    public int NumCameras => _NumCameras;

    private readonly List<GameObject> _Cameras = new();

    public IEnumerable<GameObject> Cameras => _Cameras;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((CameraLayer & (1 << collision.gameObject.layer)) != 0)
        {
            _NumCameras++;
            _Cameras.Add(collision.gameObject);
            UpdateState();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((CameraLayer & (1 << collision.gameObject.layer)) != 0)
        {
            _NumCameras--;
            _Cameras.Remove(collision.gameObject);
            UpdateState();
        }
    }

    private void UpdateState()
    {
        State.Set(_NumCameras > 0);
    }
}
