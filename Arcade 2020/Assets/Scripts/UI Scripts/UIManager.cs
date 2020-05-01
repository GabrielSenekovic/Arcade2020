using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]CanvasGroup pauseScreen;
    [SerializeField]CanvasGroup deathScreen;

    [SerializeField]EntityManager entityManager;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            OpenOrClose(pauseScreen);
        }
        //if team is both dead
        if(false)
        {
            OpenOrClose(deathScreen);
        }
    }

    void OpenOrClose(CanvasGroup UIScreen)
    {
        UIScreen.alpha = UIScreen.alpha > 0 ? 0 : 1;
        UIScreen.blocksRaycasts = !(UIScreen.blocksRaycasts); //!  = true ? false : true;
        entityManager.ToggleFreezeAllEntities(UIScreen.blocksRaycasts);
    }
}
