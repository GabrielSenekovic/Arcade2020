﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] CanvasGroup pauseScreen;

    [SerializeField] EntityManager entityManager;
    void Update()
    {

    }

    public void OpenOrClose(CanvasGroup UIScreen)
    {
        UIScreen.alpha = UIScreen.alpha > 0 ? 0 : 1;
        UIScreen.blocksRaycasts = !(UIScreen.blocksRaycasts); //!  = true ? false : true;
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void ResumeGame()
    {
        GetComponent<MenuNavigator>().enabled = false;
        OpenOrClose(pauseScreen);
    }
    public void EnterMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
    public void ExitGame()
    {
        Debug.Log("Coward");
        Application.Quit();
    }
}
