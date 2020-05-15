﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public List<GameObject> addedRooms = new List<GameObject>(){};
    [SerializeField] GameObject roomIcon;
    [SerializeField] Transform mapParent;

    public GameObject currentRoom;

    public void AddRoomToMap(Vector2 newLocation)
    {
        for(int i = 0; i < addedRooms.Count; i++)
        {
            if((Vector2)addedRooms[i].transform.position != new Vector2(newLocation.x + 12, newLocation.y + 9.5f))
            {
                GameObject newIcon = Instantiate(roomIcon, new Vector3(newLocation.x + 12, newLocation.y + 9.5f, -10), Quaternion.identity, mapParent);
                addedRooms.Add(newIcon);
                currentRoom = newIcon;
                return;
            }
            else
            {
                currentRoom = addedRooms[i];
            }
        }
    }
    public void ResetMap()
    {
        if(addedRooms.Count > 1)
        {
            for(int i = addedRooms.Count -1; i >= 0; i--)
            {
                Debug.Log(i);
                GameObject temp = addedRooms[i];
                addedRooms.RemoveAt(i);
                Destroy(temp);
            }
        }
        addedRooms.Add(Instantiate(roomIcon, new Vector3(12, 9.5f, -10), Quaternion.identity, mapParent));
        currentRoom = addedRooms[0];
    }
}
