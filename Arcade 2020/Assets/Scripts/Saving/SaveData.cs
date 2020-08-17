using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public enum HPDisplay
    {
        Hearts = 0,
        Bar = 1
    }
    public List<int> highscores = new List<int>(){};
    public float musicVolume;
    public float soundVolume;
    public HPDisplay hpDisplay;
}
