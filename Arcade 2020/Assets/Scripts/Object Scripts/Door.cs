using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector2 directionModifier = Vector2Int.zero;
    public bool locked = false;

    public Door otherDoor = null;

    [SerializeField]SpriteRenderer doorframe;
    [SerializeField]SpriteRenderer door;

    [SerializeField]Sprite[] doorSprites;
    //1: not close enough
    //2: close enough

    public void Lock()
    {
        locked = true;
        GetComponentInChildren<Animator>().SetBool("IsLocked", true);
    }
    public void Unlock()
    {
        Debug.Log("Unlocking door");
        locked = false;
        otherDoor.locked = false;
        GetComponentInChildren<Animator>().SetBool("IsLocked", false);
        GetComponentInChildren<Animator>().SetTrigger("Unlock");
    }
    public void OpenClose(bool value)
    {
        if(value)
        {
            GetComponentInChildren<Animator>().SetTrigger("Open");
        }
        else
        { 
            GetComponentInChildren<Animator>().SetTrigger("Close");
        }
    }
    public void LightUp(bool value)
    {
        doorframe.sprite = value ? doorSprites[1] : doorSprites[0];
    }
}
