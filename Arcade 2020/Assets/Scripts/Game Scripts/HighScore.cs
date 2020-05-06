using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScore : MonoBehaviour
{
    [SerializeField]Text[] highscoreText = new Text[10];

    void Start()
    {
        for(int i = 0; i < highscoreText.Length; i++)
        {
            highscoreText[i].text = (i+1) + ": 000";
        }
        for(int i = 0; i < Game.highscores.Count; i++)
        {
            highscoreText[i].text = (i+1) + ": " + Game.highscores[i];
        }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
