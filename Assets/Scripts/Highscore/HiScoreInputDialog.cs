using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class HiScoreInputDialog : MonoBehaviour
{

    [SerializeField]private List<LetterController> letters =
        new List<LetterController>();


    [SerializeField]private UserInputs userInputs;

    [SerializeField]private TMP_Text scoreText;


    private int selectedLetter = 0;

    private float score;


    void Start()
    {
        //letters[selectedLetter].StartBlink();
    }


    void OnEnable()
    {
        userInputs.Register(NextAlphaBet,AllowedInputs.EUpArray);
        userInputs.Register(PrevAlphaBet,AllowedInputs.EDownArray);
        userInputs.Register(NextLetter,AllowedInputs.ERightArray);
        userInputs.Register(PrevLetter,AllowedInputs.ELeftArray);
        userInputs.Register(StoreScore,AllowedInputs.EEnterKey);

    }

    void OnDisable()
    {
        userInputs.UnRegister(NextAlphaBet,AllowedInputs.EUpArray);
        userInputs.UnRegister(PrevAlphaBet,AllowedInputs.EDownArray);
        userInputs.UnRegister(NextLetter,AllowedInputs.ERightArray);
        userInputs.UnRegister(PrevLetter,AllowedInputs.ELeftArray);
        userInputs.UnRegister(StoreScore,AllowedInputs.EEnterKey);
    }


    public void Show(float score)
    {
        gameObject.SetActive(true);
        // 9. HiScoreInputDialog - Show() - Tallenna metodin parametrina saatu
        // muuttuja luokan jäsenmuuttujalle arvoksi
        this.score = score;



        // 10. HiScoreInputDialog - Show() - Sijoita scoreText jäsenmuuttujan 
        // text kentälle arvoksi saamasi score
        scoreText.text = score.ToString();

    }


    private void NextAlphaBet()
    {
        // 11. HiScoreInputDialog - NextAlphaBet() - Luokalla on jäsenmuuttujina
        // lista kirjaimista(Letter), kutsu listan ensimmäisen Letter
        // luokan olion NextAlphaBet Metodia
        letters[selectedLetter].NextAlphaBet();

    }


    private void PrevAlphaBet()
    {
        // 12.. HiScoreInputDialog - PrevAlphaBet() - Luokalla on jäsenmuuttujina
        // lista kirjaimista(Letter), kutsu listan ensimmäisen Letter
        // luokan olion PrevAlphaBet Metodia
        letters[selectedLetter].PrevAlphaBet();

    }

    private void NextLetter()
    {

        // 15. HiScoreInputDialog - NextLetter() - Laita seuraava kirjain 
        // vilkkumaan, ja muuta selectedLetter jäsenmuuttujan arvo
        // vastaavaan valittua kirjainta (Vinkki : letters[selectedLetter])
        //letters[selectedLetter].StopBlink();
        selectedLetter++;
        if (selectedLetter > 2) selectedLetter = 0;
        //letters[selectedLetter].StartBlink();

    }


    private void PrevLetter()
    {
        // 16. HiScoreInputDialog - PrevLetter() - Laita edellinen kirjain 
        // vilkkumaan, ja muuta selectedLetter jäsenmuuttujan arvo
        // vastaavaan valittua kirjainta (Vinkki : letters[selectedLetter])
        //letters[selectedLetter].StopBlink();
        selectedLetter--;
        if (selectedLetter < 0) selectedLetter = 2;
        //letters[selectedLetter].StartBlink();


    }

    private void StoreScore()
    {

        gameObject.SetActive(false);

        // 18.HiScoreInputDialog - StoreScore() - Luo uusi HiScoreElement
        // jolle annat rakentajametodin avulla 
        // Pelaajan juuri syöttämän nimen sekä pisteet,
        // Kutsu HiScoreStore.Instance.AddNewItemToList - metodia ja välitä
        // luomasi HiScoreElement parametrinä

        string name = letters[0].Text + letters[1].Text + letters[2].Text;
        HiScoreElement hiScoreElement = new HiScoreElement(score, name);
        HiScoreStore.Instance.AddNewItemToList(hiScoreElement);



        

    }
}
