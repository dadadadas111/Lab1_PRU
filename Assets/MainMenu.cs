using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("SampleScene");
        ScoreManager.enemiesKilled = 0;
    }

    public void Options()
    {
        Debug.Log("Options Menu!");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
