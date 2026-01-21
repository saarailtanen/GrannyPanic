using UnityEngine;
using System.Collections.Generic;

[System.Serializable] public class HiScoreList
{
    private List<HiScoreElement> hiScoreElementList =
    new List<HiScoreElement>();

    public List<HiScoreElement> HiScoreElementList
    {
        get
        {
            return this.hiScoreElementList;
        }
    }

        public void AddToList(HiScoreElement element)
    {

        // 19 HiScoreList - AddToList() - Toteuta AddToList Metodi. Metodi
        // lisää parametrina saadun HiScoreElement:in hiScoreList:aan.
        // Listassa voi olla maksimissaan 10 itemiä, ja lista järjestellään
        // pisteiden mukaan. listalla on ensimmäisenä suurimmat pisteet.
        if (hiScoreElementList.Count == 0)
        {
            hiScoreElementList.Add(
                new HiScoreElement(element.Score, element.Name));
        }

        int counter = 0;
        foreach (HiScoreElement elem in hiScoreElementList)
        {
            if (elem.Score < element.Score)
            {
                break;
            }
            counter++;
        }
        hiScoreElementList.Insert(
            counter, new HiScoreElement(element.Score, element.Name));

        if (hiScoreElementList.Count > 10)
        {
            hiScoreElementList.RemoveAt(10);
        }
        


    }
    
}

