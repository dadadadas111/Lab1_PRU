using UnityEngine;

public class PixelEffect : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Apply a stronger upward force with some horizontal variation
        Vector2 randomForce = new Vector2(Random.Range(-1f, 1f), Random.Range(3f, 5f));
        rb.AddForce(randomForce, ForceMode2D.Impulse);
    }
}
