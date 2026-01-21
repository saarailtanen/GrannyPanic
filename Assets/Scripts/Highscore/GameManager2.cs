using UnityEngine;

public class GameManager2 : MonoBehaviour {
    [SerializeField] private UserInputs userInputs;
    [SerializeField] private HiScoreInputDialog hiScoreInputDialog;
    [SerializeField] private HiScoreListDialog hiScoreListDialog;
    
    public GameTimer gameTimer;

    //tätä voi päivittää jos lisätään health?
    /*public GameHealth gameHealth;

    void Update() {
        if (!gameHealth.IsAlive()) {
            EndGame();
        }
    }
    */

    void OnEnable()
    {
        // Register to Listen User Inputs
        userInputs.Register(EndGame, AllowedInputs.ESpace);

    }

    void OnDisable()
    {
        // UnRegister to Listen User Inputs
        userInputs.UnRegister(EndGame,AllowedInputs.ESpace);
    }

    public void EndGame()
    {
        userInputs.UnRegister(EndGame, AllowedInputs.ESpace);
        gameTimer.StopTimer();
        float finalScore = CalculateScore();
        Debug.Log("Final Score: " + finalScore);
        //hiScoreInputDialog.Show(score);
        //gameText.SetActive(false);
        //tämän jälkeen näytä nimen valitsin
    }
    
        private void ToggleHiScoreList()
    {
        // 4. GameLogic - ToggleHiScoreList() - Kutsu hiScoreListDialog olion Toggle Metodia
        hiScoreListDialog.Toggle();
    }

    /*
    float CalculateScore() {
        return gameTimer.elapsedTime * gameHealth.currentHearts;
    }
    */
    float CalculateScore() {
        return gameTimer.ElapsedTime;
    }
}
