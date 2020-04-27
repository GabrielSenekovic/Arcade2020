using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector2 directionModifier = Vector2Int.zero;
    public bool locked = false;

    public Door otherDoor = null;

    [SerializeField]Sprite[] doorSprites;

    public void Lock()
    {
        locked = true;
        GetComponentInChildren<SpriteRenderer>().sprite = doorSprites[1];
    }
    public void Unlock()
    {
        locked = false;
        GetComponentInChildren<SpriteRenderer>().sprite = doorSprites[0];
        otherDoor.locked = false;
        otherDoor.GetComponentInChildren<SpriteRenderer>().sprite = doorSprites[0];
    }
}
