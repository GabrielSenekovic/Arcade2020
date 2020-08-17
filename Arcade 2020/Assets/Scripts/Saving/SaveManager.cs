using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath+"/" + "SaveTest.dat", FileMode.Create);
            SaveData saveData = new SaveData();
            SaveHighscore(saveData);
            SaveSettings(saveData);
            bf.Serialize(file, saveData);
            file.Close();
            Debug.Log("Trying to save");
        }
        catch(System.Exception)
        {
            //This is for handling errors, such as corruptions
        }
    }
    public static void Load()
    {
        Debug.Log("Loading highscores");
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath+"/" + "SaveTest.dat", FileMode.Open);
            SaveData saveData = (SaveData)bf.Deserialize(file);
            Debug.Log(saveData);
            file.Close();
            LoadHighscore(saveData);
            LoadSettings(saveData);
            foreach(int score in saveData.highscores)
            {
                Debug.Log(score);
            }
        }
        catch(System.Exception)
        {
            for(int i = 0; i < 10; i++)
            {
                Game.highscores.Add(000);
            }
            //This is for handling errors, such as corruptions or when the file couldnt be loaded
        }
    }

    static void SaveHighscore(SaveData data)
    {
        data.highscores = Game.highscores;
    }
    static void LoadHighscore(SaveData data)
    {
        Game.highscores = data.highscores;
    }
    static void SaveSettings(SaveData data)
    {
        data.soundVolume = Game.soundVolume;
        data.musicVolume = Game.musicVolume;
        data.hpDisplay = Game.hpDisplay;
    }

    static void LoadSettings(SaveData data)
    {
        Game.musicVolume = data.musicVolume;
        Game.soundVolume = data.soundVolume;
        Game.hpDisplay = data.hpDisplay;
    }
}
