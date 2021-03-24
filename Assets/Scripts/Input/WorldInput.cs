using System;
using System.Collections;
using UnityEngine;

public class WorldInput : MonoBehaviour
{
    public event Action onExit;
    
    [SerializeField] private CameraController cameraController;

    private void Update()
    {
        OnCameraMovement(new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")));
        
        if (Input.mouseScrollDelta.magnitude > 0)
        {
            OnCameraZoom(Input.mouseScrollDelta);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            StartCoroutine(Exit());
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

    private IEnumerator Exit()
    {
        onExit?.Invoke();
        yield return null;
        ApplicationController.LoadMainMenu();
    }
}
