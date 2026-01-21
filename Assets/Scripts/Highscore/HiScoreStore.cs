using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class HiScoreStore
{
    private HiScoreList list = null;
    private static HiScoreStore instance;
    public static HiScoreStore Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new HiScoreStore();
            }

            return instance;
        }
    }

    public HiScoreStore()
    {


        // 22 HiScoreStore - HiScoreStore() - Poista kommentit

        if (File.Exists(Application.persistentDataPath + "/hiscore1.save"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath +
                "/hiscore1.save", FileMode.Open);
            list = (HiScoreList)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            list = new HiScoreList();
            list.AddToList(new HiScoreElement(1, "AAA"));
            list.AddToList(new HiScoreElement(2, "BBB"));
            list.AddToList(new HiScoreElement(3, "CCC"));
            list.AddToList(new HiScoreElement(4, "DDD"));
            list.AddToList(new HiScoreElement(5, "EEE"));
            list.AddToList(new HiScoreElement(6, "FFF"));
            list.AddToList(new HiScoreElement(7, "GGG"));
            list.AddToList(new HiScoreElement(8, "HHH"));
            list.AddToList(new HiScoreElement(9, "III"));
            list.AddToList(new HiScoreElement(10, "JJJ"));
            SaveScoreBoard();
        }
    }

    public HiScoreList GetHiScoreList()
    {
        return this.list;
    }

    public void AddNewItemToList(HiScoreElement element)
    {

        // AddToList metodia, ja välitä parametrina HiScoreElement
        list.AddToList(element);


        SaveScoreBoard();

    }
    
    private void SaveScoreBoard() {

        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Open(Application.persistentDataPath + "/hiscore1.save", FileMode.OpenOrCreate);
        Debug.LogError(Application.persistentDataPath + "/hiscore1.save");
        bf.Serialize(file, list);
        file.Close();
    }


}
