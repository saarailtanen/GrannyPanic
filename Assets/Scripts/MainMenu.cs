using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public EspOhjain espOhjain;

    [SerializeField] private Button[] menuButtons; // Assign in Inspector: Play, HighScore, Exit
    private int currentButtonIndex = 0;
    private int lastPulseState = 0;
    private int pulseCount = 0;
    private const int pulsesPerStep = 3; // Adjust based on wheelchair sensitivity
    int lastButtonState = 0;
    
    void Start()
    {
        // Highlight first button on start
        if (menuButtons.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(menuButtons[0].gameObject);
        }
    }
    
    void Update()
    {
        // Detect pulse edge (0 -> 1 transition)
        int pulseState = espOhjain.espData.pulssi;
        
        if (lastPulseState == 0 && pulseState == 1)
        {
            pulseCount++;
            
            // Move to next button after enough pulses
            if (pulseCount >= pulsesPerStep)
            {
                NavigateMenu();
                pulseCount = 0;
            }
        }
        
        lastPulseState = pulseState;
        
        // Use nappi to select/click the highlighted button
        int buttonState = espOhjain.espData.nappi1;
        if (lastButtonState == 1 && buttonState == 0)
        {
            ExecuteSelectedButton();
        }
        lastButtonState = buttonState;
    }
    
    public void NavigateMenu()
    {
        // Move to next button (wrap around)
        currentButtonIndex = (currentButtonIndex + 1) % menuButtons.Length;
        EventSystem.current.SetSelectedGameObject(menuButtons[currentButtonIndex].gameObject);
    }
    
    public void ExecuteSelectedButton()
    {
        // Click the currently selected button
        Button selectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
        if (selectedButton != null)
        {
            selectedButton.onClick.Invoke();
        }
    }
    
    public void Play()
    {
        SceneManager.LoadScene("grannyPanic!LVL");
        Time.timeScale = 1f;
    }

    public void HighScore()
    {
        SceneManager.LoadScene("HighScores");
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
