using UnityEngine;

[System.Serializable]
public class HiScoreElement
{


    // 1. HiScoreElement - Luo luokalle score(float) ja name(string) jäsenmuuttuja
    private float score;

    private string name = " ";

    // 2. HiScoreElement - Tee Get ominaisuudet luomillesi jäsenmuuttujille

    //pitää olla property tai getter jotta päästään käsiksi jäesnmuuttujiin
    public string Name
    {
        get
        {
            return this.name;
        }
    }

    public float Score
    {
        get
        {
            return this.score;
        }
    }

    // 17. HiScoreElement - Luo luokalle rakentaja joka ottaa parametrina pisteet 
    // ja nimen sekä sijoittaa ne luokan jäsenmuuttujille arvoksi
    public HiScoreElement(float score, string name)
    {
        this.score = score;
        this.name = name;
    }
}
