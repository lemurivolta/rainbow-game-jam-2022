using UnityEngine;

public class CameraStateUpdater : MonoBehaviour
{
    public LayerMask CameraLayer;

    public State State;

    private int _NumCameras = 0;

    public int NumCameras => _NumCameras;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((CameraLayer & (1 << collision.gameObject.layer)) != 0)
        {
            _NumCameras++;
            UpdateState();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((CameraLayer & (1 << collision.gameObject.layer)) != 0)
        {
            _NumCameras--;
            UpdateState();
        }
    }

    private void UpdateState()
    {
        //Debug.Log($"Setting {gameObject.transform.parent.gameObject.name} is now under {_NumCameras} cameras.");
        State.Set(_NumCameras > 0);
    }
}
