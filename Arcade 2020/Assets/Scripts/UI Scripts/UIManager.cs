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

    [System.NonSerialized] public Color[] colors = new Color[2];

    [SerializeField]EntityManager entityManager;
    
    void Awake()
    {
        colors[0] = new Color(0.016f, 0.32f, 1);
        colors[1] = new Color(colors[0].r + 0.2f,colors[0].g + 0.4f,colors[0].b,1);
    }
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
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public IEnumerator RevealMap(float time, bool roomCleared)
    {
        minimap.gameObject.SetActive(true);
        SpriteRenderer mapRend = minimap.currentRoom.GetComponent<SpriteRenderer>();

        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSecondsRealtime(time/6);
            mapRend.color = colors[1];
            yield return new WaitForSecondsRealtime(time/6);
            mapRend.color = colors[0];
        }
        if(!roomCleared)
        { 
            minimap.gameObject.SetActive(false);
        }
        else
        {
            mapRend.color = colors[1];
        }
    }
}
