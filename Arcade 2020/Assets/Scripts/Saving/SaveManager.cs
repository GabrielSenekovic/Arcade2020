using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath+"/" + "SaveTest.dat", FileMode.OpenOrCreate);
            SaveData saveData = new SaveData();
            bf.Serialize(file, saveData);
            file.Close();
        }
        catch(System.Exception)
        {
            //This is for handling errors, such as corruptions
        }
    }
    void Load()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath+"/" + "SaveTest.dat", FileMode.Open);
            SaveData saveData = (SaveData)bf.Deserialize(file);
            file.Close();
            LoadHighscore(saveData);
        }
        catch(System.Exception)
        {
            //This is for handling errors, such as corruptions
        }
    }

    void SaveHighscore(SaveData data)
    {

    }
    void LoadHighscore(SaveData data)
    {

    }
}
