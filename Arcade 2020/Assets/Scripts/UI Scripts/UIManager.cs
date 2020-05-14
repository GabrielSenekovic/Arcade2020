using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]CanvasGroup pauseScreen;

    public CanvasGroup speechBubble;
    public SpeechBubble speechBubble_Obj;
    public CanvasGroup deathScreen;
    public Minimap minimap;

    public Score score;

    public Text floorText;

    [SerializeField]EntityManager entityManager;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(speechBubble.alpha == 1 && speechBubble_Obj.messageDone)
            {
                OpenOrClose(speechBubble);
                speechBubble.GetComponentInChildren<Text>().text = "";
                minimap.gameObject.SetActive(true);
            }
            else
            {
                OpenOrClose(pauseScreen);
            }
        }
    }

    public void OpenOrClose(CanvasGroup UIScreen)
    {
        UIScreen.alpha = UIScreen.alpha > 0 ? 0 : 1;
        UIScreen.blocksRaycasts = !(UIScreen.blocksRaycasts); //!  = true ? false : true;
        entityManager.ToggleFreezeAllEntities(UIScreen.blocksRaycasts);
    }

    public IEnumerator RevealMap(float time, bool roomCleared)
    {
        minimap.gameObject.SetActive(true);
        SpriteRenderer mapRend = minimap.currentRoom.GetComponent<SpriteRenderer>();
        Color originalColor = mapRend.color;

        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSecondsRealtime(time/6);
            mapRend.color = new Color(originalColor.r + 0.2f,originalColor.g + 0.4f,originalColor.b,1);
            yield return new WaitForSecondsRealtime(time/6);
            mapRend.color = originalColor;
        }
        if(!roomCleared)
        { 
            minimap.gameObject.SetActive(false);
        }
    }
}
