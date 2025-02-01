using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign the player in the Inspector
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Camera offset (z = -10 for 2D)
    public float smoothSpeed = 0.125f; // How smoothly the camera follows

    public float fixedYPosition = 0f; // Lock the camera's Y position to this value
    public int pixelsPerUnit = 16; // Match your sprite's Pixels Per Unit (PPU)

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the desired camera position (only follow X, ignore Y)
            Vector3 desiredPosition = new Vector3(player.position.x, fixedYPosition, 0f) + offset;

            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Snap the camera to pixel-perfect positions AFTER smoothing
            smoothedPosition.x = Mathf.Round(smoothedPosition.x * pixelsPerUnit) / pixelsPerUnit;
            smoothedPosition.y = Mathf.Round(smoothedPosition.y * pixelsPerUnit) / pixelsPerUnit;

            // Apply the smoothed and snapped position to the camera
            transform.position = smoothedPosition;
        }
    }
}