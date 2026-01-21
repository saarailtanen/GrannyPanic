using UnityEngine;
using TMPro;

public class HiScoreListElement : MonoBehaviour
{
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text playerName;
    
    
    public void SetData(HiScoreElement element)
    {
        //3. HiScoreListElement - SetData() - tallenna jäsenmuuttujien arvoksi HiScoreElement luokan Score ja Name
        score.text = element.Score.ToString();
        playerName.text = element.Name.ToString();
    }
}
