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

    public CanvasGroup ballSwitching;

    public Score score;

    public Text floorText;

    public Cursor cursor;

    [System.NonSerialized] public Color[] colors = new Color[2];
    
    void Awake()
    {
        colors[0] = new Color(0.016f, 0.32f, 1);
        colors[1] = new Color(colors[0].r + 0.2f,colors[0].g + 0.4f,colors[0].b,1);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(ballSwitching.alpha == 1)
            {
                OpenOrClose(ballSwitching);
                ballSwitching.GetComponent<BallSwitching>().Close();
            }
            else if(speechBubble.alpha == 1)
            {
                if(speechBubble_Obj.dialogDone)
                {
                    if(speechBubble_Obj.messageDone)
                    {
                        OpenOrClose(speechBubble);
                        speechBubble.GetComponentInChildren<Text>().text = "";
                        minimap.gameObject.SetActive(true);
                    }
                    else
                    {
                        speechBubble_Obj.breakPrint = true;
                    }
                }
                else if(!speechBubble_Obj.dialogDone)
                {
                    if(speechBubble_Obj.messageDone && speechBubble_Obj.lineNumber > 0)
                    {
                        speechBubble_Obj.ContinueDialog();
                    }
                    else if(!speechBubble_Obj.messageDone)
                    {
                        speechBubble_Obj.breakPrint = true;
                    }
                }
            }
            else if(speechBubble.alpha == 0)
            {
                OpenOrClose(pauseScreen);
            }
        }
    }

    public void OpenBallSwitching(PlayerBallController player, Ball pickedUpBall)
    {
        OpenOrClose(ballSwitching);
        ballSwitching.GetComponent<BallSwitching>().Open(player, pickedUpBall);
    }

    public void OpenOrClose(CanvasGroup UIScreen)
    {
        UIScreen.alpha = UIScreen.alpha > 0 ? 0 : 1;
        UIScreen.blocksRaycasts = !(UIScreen.blocksRaycasts); //!  = true ? false : true;
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        cursor.gameObject.SetActive(cursor.gameObject.activeSelf ? false: true);
        if(UIScreen.transform.parent.GetComponent<MenuNavigator>())
        {
            UIScreen.transform.parent.GetComponent<MenuNavigator>().enabled = UIScreen.blocksRaycasts;
        }
    }

    public IEnumerator RevealMap(float time, bool roomCleared)
    {
        minimap.gameObject.SetActive(true);
        SpriteRenderer mapRend = minimap.currentRoom.GetComponent<SpriteRenderer>();

        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSecondsRealtime(time/6);
            if(mapRend == null)
            {
                break;
            }
            mapRend.color = colors[1];
            yield return new WaitForSecondsRealtime(time/6);
            if(mapRend == null)
            {
                break;
            }
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
