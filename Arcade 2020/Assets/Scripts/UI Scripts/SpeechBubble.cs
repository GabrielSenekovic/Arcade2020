using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField]float speechDelay;
    public bool messageDone = false;
    public bool breakPrint = false;
    public IEnumerator PrintMessage(string text)
    {
        messageDone = false;
        foreach(char c in text)
        {
            if(breakPrint)
            {
                breakPrint = false;
                GetComponentInChildren<Text>().text = text;
                break;
            }
            if(c != ' ')
            {
                yield return new WaitForSecondsRealtime(speechDelay);
            }
            GetComponentInChildren<Text>().text += c;
        }
        messageDone = true;
    }
}
