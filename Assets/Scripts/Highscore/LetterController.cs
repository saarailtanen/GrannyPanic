using UnityEngine;
using System;
using TMPro;

public class LetterController : MonoBehaviour
{
    private TMP_Text textComponent;
    public String Text
    {
        get
        {
            return textComponent.text;
        }
    }


    public void NextAlphaBet()
    {

        // 14. LetterController - NextAlphaBet() - 
        // Valitulle kirjaimelle annetaan arvoksi seuraava kirjain aakkosista. 
        // (Vinkki textComponent.text muuttujaan on sijoitettu nykyinen kirjain)
        char c = textComponent.text.ToCharArray()[0];

        c++;
        if (c > (int)'Z') c = 'A';
        textComponent.text = c.ToString();
    }
    
    public void PrevAlphaBet()
    {

        // 13. LetterController - PrevAlphaBet() - 
        // Valitulle kirjaimelle annetaan arvoksi edellinen kirjain aakkosista. 
        // (Vinkki textComponent.text muuttujaan on sijoitettu nykyinen kirjain)
        char c = textComponent.text.ToCharArray()[0];

        c--;
        if (c < (int)'A') c = 'Z';
        textComponent.text = c.ToString();
    }

}
