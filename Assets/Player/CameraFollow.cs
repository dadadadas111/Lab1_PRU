using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;
    public SpriteRenderer background;

    private float minX, maxX, minY, maxY;
    private float camHalfWidth, camHalfHeight;

    void Start()
    {
        if (background == null)
        {
            Debug.LogError("Background is not assigned!");
            return;
        }

        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;

        // Get the background size based on the sprite
        float bgHalfWidth = background.bounds.size.x / 2f;
        float bgHalfHeight = background.bounds.size.y / 2f;

        // Calculate camera movement limits
        minX = background.transform.position.x - bgHalfWidth + camHalfWidth;
        maxX = background.transform.position.x + bgHalfWidth - camHalfWidth;
        minY = background.transform.position.y - bgHalfHeight + camHalfHeight;
        maxY = background.transform.position.y + bgHalfHeight - camHalfHeight;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate new target position
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        newPos = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);

        // Clamp the camera position within background bounds
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        transform.position = newPos;
    }
}
