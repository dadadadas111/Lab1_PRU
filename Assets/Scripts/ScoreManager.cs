using UnityEngine;
using TMPro; // Required for TextMesh Pro

public class ScoreManager : MonoBehaviour
{
    public static int enemiesKilled = 0; // Tracks the number of enemies killed
    private TMP_Text textMeshPro; // Reference to the TMP text component

    void Start()
    {
        // Get the TMP_Text component from this GameObject
        textMeshPro = GetComponent<TMP_Text>();

        if (textMeshPro == null)
        {
            Debug.LogError("TMP_Text component missing on " + gameObject.name);
            return;
        }

        UpdateText(); // Initialize text
    }

    // Increases the kill counter and updates text
    public void IncreaseKillCount()
    {
        enemiesKilled++;
        UpdateText();
    }

    // Resets the counter and updates text
    public void ResetKillCount()
    {
        enemiesKilled = 0;
        UpdateText();
    }

    // Updates the TMP text with the current score
    private void UpdateText()
    {
        textMeshPro.text = "X " + enemiesKilled;
    }
}
