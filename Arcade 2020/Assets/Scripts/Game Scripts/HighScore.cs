using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    int[] highscores = new int[10];
    [SerializeField]Text[] highscoreText = new Text[10];

    void Awake()
    {
        for(int i = 0; i < highscores.Length; i++)
        {
            highscores[i] = 100;
            highscoreText[i].text = (i+1) + ": " + highscores[i];
        }
    }
}
