using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuScript : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelGenerationTest");
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
