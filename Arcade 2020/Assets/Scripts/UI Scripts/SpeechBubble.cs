using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField]float speechDelay;
    public bool messageDone = false;

    public bool dialogDone = false;
    public bool breakPrint = false;

    Manuscript.Dialog currentDialog;

    int lineIndex = 0;

    public void InitiateDialog(Manuscript.Dialog dialog)
    {
        dialogDone = false;
        currentDialog = dialog;
        messageDone  = true;
        ContinueDialog();
    }
    public void Say(Manuscript.Dialog.Line line)
    {
        dialogDone = true;
        StartCoroutine(PrintMessage(line.myLine));
    }

    public void ContinueDialog()
    {
        if(messageDone)
        {
            GetComponentInChildren<Text>().text = "";
            StartCoroutine(PrintMessage(currentDialog.myLines[lineIndex].myLine));
            lineIndex++;
            if(lineIndex > currentDialog.myLines.Count - 1)
            {
                dialogDone = true;
                lineIndex = 0;
            }
        }
    }
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
