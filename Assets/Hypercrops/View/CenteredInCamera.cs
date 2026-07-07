using UnityEngine;

public class CenteredInCamera : MonoBehaviour
{
    public Transform targetObject;
    [SerializeField] private float smoothSpeed; // How smoothly the camera follows (0 is instant, higher values are smoother)
    [SerializeField] private Vector3 offset;

    void Start()
    {
        smoothSpeed = 1f;
    }

    void LateUpdate()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("CameraFollow: Target is not assigned.");
            return; // Exit if there is no target
        }

        // Calculate the desired position of the camera
        Vector3 desiredPosition = targetObject.position + offset;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position
        transform.position = smoothedPosition;

        // (Optional)  Keep the camera looking at the target (for a third-person view)
        // transform.LookAt(targetObject);
    }
}
