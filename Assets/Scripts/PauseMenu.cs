using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject PauseMenuUI;
    [SerializeField] private Button menuButton;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color pausedColor = Color.gray;
    [SerializeField] private Button[] menuButtons; // Assign in Inspector: Play, HighScore, Exit
    private bool isPaused = false;

    private int currentButtonIndex = 0;
    private int lastPulseState = 0;
    private int pulseCount = 0;
    private const int pulsesPerStep = 3; // Adjust based on wheelchair sensitivity
 
    public GameManager gameManager;

    [SerializeField]
    GameObject nimi;

    public EspOhjain espOhjain;

    int lastButtonState;

    void Start()
    {
    PauseMenuUI.SetActive(false);
    isPaused = false;
    lastButtonState = 0; // Initialize to unpressed state

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
        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Resume();
                Time.timeScale = 1f;
            }
            else
            {
                Pause();
                Time.timeScale = 0f;
            }
        }
*/
        
    int buttonState = espOhjain.espData.nappi1;
    
    // Detect falling edge (button press: 1 -> 0)
    if (lastButtonState == 1 && buttonState == 0)
    {
        if (isPaused)
        {
            Resume();
            ExecuteSelectedButton();
        }
        else
        {
            Pause();
        }
    }
    
    lastButtonState = buttonState; // Always update at the end
        
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        
        if (menuButton != null)
        {
            var colors = menuButton.colors;
            colors.normalColor = normalColor;
            menuButton.colors = colors;
        }
        if (timerText != null)
        {
            timerText.color = normalColor;
        }
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        if (menuButton != null)
        {
            var colors = menuButton.colors;
            colors.normalColor = pausedColor;
            menuButton.colors = colors;
        }
        if (timerText != null)
        {
            timerText.color = pausedColor;
        }
    }
    
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
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

    public void NavigateMenu()
    {
        // Move to next button (wrap around)
        currentButtonIndex = (currentButtonIndex + 1) % menuButtons.Length;
        EventSystem.current.SetSelectedGameObject(menuButtons[currentButtonIndex].gameObject);
    }

}
