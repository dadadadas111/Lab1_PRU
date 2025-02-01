using System.Collections;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public Transform linkedPortal; // Assign the other portal in the Inspector
    public float teleportCooldown = 0.5f; // Prevent instant re-entry

    private bool isTeleporting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTeleporting && other.CompareTag("Player"))
        {
            StartCoroutine(Teleport(other));
        }
    }

    private IEnumerator Teleport(Collider2D player)
    {
        isTeleporting = true;

        // Move player to linked portal position + slight offset to prevent instant re-entry
        player.transform.position = new Vector2(linkedPortal.position.x, linkedPortal.position.y + 0.5f);

        yield return new WaitForSeconds(teleportCooldown);

        isTeleporting = false;
    }
}
