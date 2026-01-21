using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EnterName : MonoBehaviour
{
    public TMP_Text[] letterSlots;
    public TMP_Text scoreText;

    private int[] letterIndices = { 0, 0, 0};
    private int currentSlot = 0;
    private bool isConfirmed = false;

    private char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    public EspOhjain espOhjain;

    public EspOhjainVasen espOhjainVasen;

    GameObject pausemenu;

    int lastButtonState = 1;

    void Start()
    {
        pausemenu = GameObject.Find("UI");
    }

    private void OnEnable() {
        UpdateLetters();
        int score = PlayerPrefs.GetInt("lastScore", 0);
        if(scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    private void Update()
    {

        pausemenu.SetActive(false);

        if (isConfirmed) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            letterIndices[currentSlot] = (letterIndices[currentSlot] + 1) % alphabet.Length;
            UpdateLetters();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            letterIndices[currentSlot]--;
            if (letterIndices[currentSlot] < 0) letterIndices[currentSlot] = alphabet.Length - 1;
            UpdateLetters();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentSlot = Mathf.Min(currentSlot + 1, letterSlots.Length - 1);
            UpdateLetters();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentSlot = Mathf.Max(currentSlot - 1, 0);
            UpdateLetters();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmName();
        }


        //OHJAIMELLA KIRJAINTEN VALITSEMINEN
        //pyörittää pyörää 1 pulse = 1 kirjain
        //jos kytkin jos kytkin 1 niin menee ylös
        if (espOhjain.espData.pulssi == 1)
        {
            letterIndices[currentSlot] = (letterIndices[currentSlot] + 1) % alphabet.Length;
            UpdateLetters();
        }


        int buttonState = espOhjain.espData.nappi1;
        //jos nappi niin menee yhen eteenpäin  ja sitku reachaa vikan kirjaimen niin seuraava on confirmname

        if (lastButtonState == 1 && buttonState == 0)
        {
            currentSlot++;
            
            if (currentSlot >= letterSlots.Length)
            {
                ConfirmName();
            }
            else
            {
                UpdateLetters();
            }
        }
        lastButtonState = buttonState;

    }

    private void UpdateLetters()
    {
        for (int i = 0; i < letterSlots.Length; i++)
        {
            letterSlots[i].text = alphabet[letterIndices[i]].ToString();
            letterSlots[i].color = (i == currentSlot) ? Color.yellow : Color.white;
        }
    }

    private void ConfirmName()
    {
        isConfirmed = true;

        string playerName = "";
        foreach (int index in letterIndices)
            playerName += alphabet[index];

        //kutsutaan playerprefs et saadaan viimisin score
        int score = PlayerPrefs.GetInt("lastScore", 0);

        Highscores.AddHighscoreEntry(score, playerName);
        PlayerPrefs.Save();

        Debug.Log("Saving score: " + score + " Name: " + playerName);
        Debug.Log("Highscore JSON: " + PlayerPrefs.GetString("highscoreTable"));

        SceneManager.LoadScene("HighScores");
    }
}