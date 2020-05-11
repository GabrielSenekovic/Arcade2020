using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuScript : MonoBehaviour
{
    public void RestartGame()
    {
        Debug.Log("Yeah, this restarts the game");
        SceneManager.LoadScene("LevelGenerationTest");
    }

    public void EnterMainMenu()
    {
        Debug.Log("Main menu time");
         SceneManager.LoadScene("MainMenuScene");
    }
        public void ExitGame()
    {
        Debug.Log("Coward");
        Application.Quit();
    }
}
