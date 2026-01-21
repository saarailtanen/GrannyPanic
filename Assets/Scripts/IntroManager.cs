using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{

    public EspOhjain espOhjain;
    [SerializeField] GameObject GameUI;
    [SerializeField] GameObject VideoUI;
    [SerializeField] GameObject PauseMenuUI;
    [SerializeField] GameObject TeksPanel;

    int lastButtonState = 0;
    
    float kulunutAika = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TeksPanel.SetActive(true);
        VideoUI.SetActive(true);
        GameUI.SetActive(false);
        Time.timeScale = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        int buttonState = espOhjain.espData.nappi1;
        kulunutAika += Time.unscaledDeltaTime;

        if (kulunutAika > 20f)
        {
            GameUI.SetActive(true);
            VideoUI.SetActive(false);
        }
        if (lastButtonState == 1 && buttonState == 0)
        {
            GameUI.SetActive(true);
            VideoUI.SetActive(false);
            PauseMenuUI.SetActive(false);
            TeksPanel.SetActive(false);
            Time.timeScale = 1f;
            this.enabled = false;
        }
        lastButtonState = buttonState;
    }
}
