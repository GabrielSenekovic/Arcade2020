using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField]float speechSpeed;
    public bool messageDone = false;
    public IEnumerator PrintMessage(string text)
    {
        messageDone = false;
        foreach(char c in text)
        {
            if(c != ' ')
            {
                yield return new WaitForSecondsRealtime(speechSpeed);
            }
            GetComponentInChildren<Text>().text += c;
        }
        messageDone = true;
    }
}
