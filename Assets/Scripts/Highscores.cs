using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Highscores : MonoBehaviour
{

    public EspOhjain espOhjain;
    
    [SerializeField] private Button[] menuButtons; // Assign Back and Play buttons
    private int currentButtonIndex = 0;
    private int lastPulseState = 0;
    private int pulseCount = 0;
    private const int pulsesPerStep = 3;
    int lastButtonState = 0;

    //NAVIGOINTI
    void Start()
    {
        // Select first button so navigation works
        if (menuButtons.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(menuButtons[0].gameObject);
        }
    }
    
    void Update()
    {
        // Wheelchair pulse navigation
        int pulseState = espOhjain.espData.pulssi;
        
        if (lastPulseState == 0 && pulseState == 1)
        {
            pulseCount++;
            
            if (pulseCount >= pulsesPerStep)
            {
                NavigateMenu();
                pulseCount = 0;
            }
        }
        
        lastPulseState = pulseState;
        
        // Use nappi1 to select/click the highlighted button
        int buttonState = espOhjain.espData.nappi1;
        if (lastButtonState == 1 && buttonState == 0)
        {
            ExecuteSelectedButton();
        }
        lastButtonState = buttonState;
    }
    
    private void NavigateMenu()
    {
        currentButtonIndex = (currentButtonIndex + 1) % menuButtons.Length;
        EventSystem.current.SetSelectedGameObject(menuButtons[currentButtonIndex].gameObject);
    }
    
    private void ExecuteSelectedButton()
    {
        Button selectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
        if (selectedButton != null)
        {
            selectedButton.onClick.Invoke();
        }
    }
    
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Play()
    {
        SceneManager.LoadScene("grannyPanic!LVL");
        Time.timeScale = 1f;
    }

    //NAVIGOINTI LOPPUU

    [SerializeField] Transform entryContainer;
    [SerializeField] Transform entryTemplate;

    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        //jos haluu deletee koko highscore listan tee näin:
        //PlayerPrefs.DeleteKey("highscoreTable");
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("highscoreTable", "{}");
        HighscoresAll highscores = JsonUtility.FromJson<HighscoresAll>(jsonString);

        if (highscores != null && highscores.highscoreEntryList != null)
        {
            highscoreEntryList = highscores.highscoreEntryList;
        }
        else
        {
            highscoreEntryList = new List<HighscoreEntry>();
        }
        //lajitellaan kun ladataan lista
        for (int i =0; i< highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscoreEntryList.Count; j++)
            {
                if(highscoreEntryList[j].score > highscoreEntryList[i].score)
                {
                    //swap
                    HighscoreEntry tmp = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j];
                    highscoreEntryList[j] = tmp;
                }
            }
        }
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHighScoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHighScoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;

        }
        entryTransform.Find("posText (1)").GetComponent<TextMeshProUGUI>().text = rankString;

        int score = highscoreEntry.score;

        entryTransform.Find("scoreText (1)").GetComponent<TextMeshProUGUI>().text = score.ToString();

        string nameText = highscoreEntry.name;
        entryTransform.Find("nameText (1)").GetComponent<TextMeshProUGUI>().text = nameText;

        transformList.Add(entryTransform);
    }
    
    public static void AddHighscoreEntry(int score, string name)
    {
        //create highscore entry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        //load saved highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable", "{}");
        HighscoresAll highscores = JsonUtility.FromJson<HighscoresAll>(jsonString);

        // make sure we have a list to work with
        if (highscores == null) highscores = new HighscoresAll();
        if (highscores.highscoreEntryList == null) highscores.highscoreEntryList = new List<HighscoreEntry>();
        //add new entry to highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        highscores.highscoreEntryList.Sort((a,b) => b.score.CompareTo(a.score));

        if(highscores.highscoreEntryList.Count > 10)
        {
            highscores.highscoreEntryList = highscores.highscoreEntryList.GetRange(0,10);
        }

        //save
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();

    }
    private class HighscoresAll{
        public List<HighscoreEntry> highscoreEntryList;
    }
    
    //yksittäinen entry

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
}
