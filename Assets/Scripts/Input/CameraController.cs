using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float zoomStep;

    private void Start()
    {
        transform.LookAt(cameraPivot);
    }
    
    public void Zoom(bool zoomIn)
    {
        var direction = cameraPivot.transform.position - transform.position;
        var distance = Vector3.Distance(transform.position, cameraPivot.position);
        if (zoomIn &&  distance > minDistance)
        {
            transform.Translate(direction * zoomStep, Space.World);
        }
        else if (!zoomIn &&  distance < maxDistance)
        {
            transform.Translate(direction * -zoomStep, Space.World);
        }
    }

    public void Move(Vector2 moveDirection)
    {
        cameraPivot.transform.Rotate(new Vector3(moveDirection.y, -moveDirection.x, 0), Space.World);
        var rotationEulerAngles = cameraPivot.transform.rotation.eulerAngles;
        rotationEulerAngles.z = 0;
        cameraPivot.transform.rotation = Quaternion.Euler(rotationEulerAngles);
    }
}
