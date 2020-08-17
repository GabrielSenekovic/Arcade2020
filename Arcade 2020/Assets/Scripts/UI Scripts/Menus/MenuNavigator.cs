using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuNavigator : MonoBehaviour
{
    public Button[] buttons;
    int currentIndex = 0;

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(currentIndex == 0)
            {
                currentIndex = buttons.Length-1;
            }
            else
            {
                currentIndex--;
            }
            buttons[currentIndex].Select();
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentIndex == buttons.Length -1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            buttons[currentIndex].Select();
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(GetComponentInChildren<CanvasGroup>())
            {
                if(GetComponentInChildren<CanvasGroup>().alpha > 0)
                {
                    ExecuteEvents.Execute(buttons[currentIndex].gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
                }
            }
            else
            {
                ExecuteEvents.Execute(buttons[currentIndex].gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
        }
    }
}
