using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject GameOverMenuUI;
    public TMPro.TMP_Text result;

    // Start is called before the first frame update
    void Start()
    {
        GameOverMenuUI.SetActive(false);
        //result = GameOverMenuUI.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    GameOverMenuUI.SetActive(true);
        //    Time.timeScale = 0f;
        //}
    }

    // on game over
    public void GameOver()
    {
        GameOverMenuUI.SetActive(true);
        var enemiesKilled = ScoreManager.enemiesKilled;
        if (result != null)
        {
            if (enemiesKilled == 0)
            {
                result.text = "You were defeated!";
            }
            else
            {
                result.text = "You defeated " + enemiesKilled + " enemies!";
            }
        }
        Time.timeScale = 0f;
    }

    // on restart
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
        ScoreManager.enemiesKilled = 0;
        Time.timeScale = 1f;
    }

    // on quit
    public void Quit()
    {
        Application.Quit();
    }
}
