using UnityEngine;

public class PixelPerfectMovement : MonoBehaviour
{
    public int pixelsPerUnit = 16; // Match with your sprite's PPU

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Round(pos.x * pixelsPerUnit) / pixelsPerUnit;
        pos.y = Mathf.Round(pos.y * pixelsPerUnit) / pixelsPerUnit;
        transform.position = pos;
    }
}
