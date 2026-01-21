using UnityEngine;

public class HiScoreListDialog : MonoBehaviour
{

    [SerializeField] private GameObject hiScoreTable;

    [SerializeField] private GameObject hiScoreElement;

    public void Toggle()
    {
        // 5. HiScoreListDialog - Toggle() - Tee Toggle toiminnallisuus, joka 
        // piilottaa HiScore dialogin jos dialogi on aktiivisena ja vastaavasti
        // Näyttää dialogin jos dialogi on piilossa. Vinkki SetAcive() - Metodi

        gameObject.SetActive(!gameObject.activeSelf);

        UpdateList();
    }

    private void UpdateList()
    {

        // 23.HiScoreListDialog - UpdateList() - Poista kaikki child 
        // GameObjektit hiScoreTable(HiScoreTable_Image)
        // GameObjectin alta
        foreach (Transform child in hiScoreTable.transform)
        {
            Destroy(child.gameObject);
        }


        HiScoreList list = HiScoreStore.Instance.GetHiScoreList();

        foreach (HiScoreElement elem in list.HiScoreElementList)
        {
            GameObject tmp = GameObject.Instantiate(hiScoreElement,
                hiScoreTable.transform);
            tmp.SetActive(true);
            tmp.GetComponent<HiScoreListElement>().SetData(elem);
        }

    }
}
