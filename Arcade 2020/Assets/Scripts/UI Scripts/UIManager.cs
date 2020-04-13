using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]CanvasGroup pauseScreen;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            OpenOrClose();
        }
    }

    void OpenOrClose()
    {
        Debug.Log("Trying to turn on and off pause screen");
        pauseScreen.alpha = pauseScreen.alpha > 0 ? 0 : 1;
        pauseScreen.blocksRaycasts = pauseScreen.blocksRaycasts == true ? false : true;
    }
}
