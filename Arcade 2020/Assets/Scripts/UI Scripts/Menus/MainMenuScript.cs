﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("play");
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ShowHighScores()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Debug.Log("Please do not leave me like this");
        SaveManager.Save();
        Application.Quit();
    }
}
