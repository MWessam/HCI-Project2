using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] float _smoothSpeed = 5f; // Speed of smoothing
    private Camera _mainCamera;     // Reference to the main camera

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.nearClipPlane));

        // Keep the crosshair on the same Z-plane
        mousePosition.z = transform.position.z;

        // Smoothly interpolate the crosshair's position toward the mouse position
        transform.position = Vector3.Lerp(transform.position, mousePosition, _smoothSpeed * Time.deltaTime);
    }
}