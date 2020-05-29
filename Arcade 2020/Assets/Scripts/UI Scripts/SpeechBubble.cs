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

    public Sprite[] sprites;

    public int lineIndex = 0;
    public int lineNumber = 0;

    public void InitiateDialog(Manuscript.Dialog dialog)
    {
        lineIndex = 0;
        dialogDone = false;
        currentDialog = dialog;
        messageDone  = true;
        lineNumber = dialog.myLines.Count;
        ContinueDialog();
    }
    public void Say(Manuscript.Dialog.Line line)
    {
        lineIndex++;
        dialogDone = true;
        if(line.myIdentity == Manuscript.Dialog.Line.CharacterIdentity.P1)
        {
            GetComponent<Image>().sprite = sprites[0];
        }
        else
        {
            GetComponent<Image>().sprite = sprites[1];
        }
        StartCoroutine(PrintMessage(line.myLine));
    }

    public void Say(Manuscript.Dialog lines, int amountOfLines)
    {
        dialogDone = false;
        messageDone = true;
        lineNumber = amountOfLines;
        currentDialog = lines;
        ContinueDialog();
    }

    public void ContinueDialog()
    {
        if(messageDone)
        {
            if(currentDialog.myLines[lineIndex].myIdentity == Manuscript.Dialog.Line.CharacterIdentity.P1)
            {
                GetComponent<Image>().sprite = sprites[0];
            }
            else
            {
                GetComponent<Image>().sprite = sprites[1];
            }
            GetComponentInChildren<Text>().text = "";
            StartCoroutine(PrintMessage(currentDialog.myLines[lineIndex].myLine));
            lineIndex++;
            lineNumber--;
            if(lineNumber == 0)
            {
                dialogDone = true;
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
