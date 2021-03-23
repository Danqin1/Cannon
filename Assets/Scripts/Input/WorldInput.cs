using UnityEngine;

public class WorldInput : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;

    private void Update()
    {
        OnCameraMovement(new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")));
        if (Input.mouseScrollDelta.magnitude > 0)
        {
            OnCameraZoom(Input.mouseScrollDelta);
        }
    }

    private void OnCameraMovement(Vector2 move)
    {
        cameraController.Move(move);
    }

    private void OnCameraZoom(Vector2 zoom)
    {
        cameraController.Zoom(zoom.y > 0);
    }
}
