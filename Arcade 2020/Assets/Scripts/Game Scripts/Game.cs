﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    static public List<int> highscores = new List<int>(){};
    static bool created = false;
    static public float musicVolume = 0;
    static public float soundVolume = 0;
    static public SaveData.HPDisplay hpDisplay;

    void Awake()
    {
        if(created)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            SaveManager.Load();
        }
    }

    public static void SaveHighScore(int newScore)
    {
        Debug.Log("Saving Highscore");
        for(int i = 0; i < highscores.Count; i++)
        {
            if(newScore > highscores[i])
            {
                highscores.Insert(i, newScore);
                break;
            }
        }
        if(highscores.Count > 10)
        {
            for(int i = highscores.Count -1; i > 9; i--)
            {
                highscores.RemoveAt(i);
            }
        }
    }
}
