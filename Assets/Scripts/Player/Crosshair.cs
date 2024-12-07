using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] float _smoothSpeed = 5f; // Speed of smoothing
    private Camera _mainCamera;     // Reference to the main camera
    [SerializeField] private Transform _player;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.nearClipPlane));

        // Keep the crosshair on the same Z-plane
        mousePosition.z = 0;

        // Smoothly interpolate the crosshair's position toward the mouse position
        transform.position = Vector3.Lerp(transform.position, mousePosition, _smoothSpeed * Time.deltaTime);

        var direction = mousePosition - _player.position;
        // Calculate the target angle (in degrees)
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create a smooth rotation towards the target angle
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        _player.rotation = Quaternion.Lerp(_player.rotation, targetRotation, _smoothSpeed * Time.deltaTime);
        

    }
}