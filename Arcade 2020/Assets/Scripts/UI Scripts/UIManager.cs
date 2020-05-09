using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]CanvasGroup pauseScreen;
    public CanvasGroup deathScreen;
    public Minimap minimap;

    public Score score;

    public Text floorText;

    [SerializeField]EntityManager entityManager;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            OpenOrClose(pauseScreen);
        }
    }

    public void OpenOrClose(CanvasGroup UIScreen)
    {
        UIScreen.alpha = UIScreen.alpha > 0 ? 0 : 1;
        UIScreen.blocksRaycasts = !(UIScreen.blocksRaycasts); //!  = true ? false : true;
        entityManager.ToggleFreezeAllEntities(UIScreen.blocksRaycasts);
    }
}
