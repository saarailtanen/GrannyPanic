using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject EnterNameCanvas;
    private GameTimer gameTimer; 

    private void Start()
    {
        gameTimer = Object.FindFirstObjectByType<GameTimer>();
        //EnterNameCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndGame();
        }
    }

    internal void EndGame()
    {
        Time.timeScale = 0f;
        gameTimer.StopTimer();
        int score = gameTimer.GetScore();
        PlayerPrefs.SetInt("lastScore", score);
        PlayerPrefs.Save();

        EnterNameCanvas.SetActive(true);
    }

}

