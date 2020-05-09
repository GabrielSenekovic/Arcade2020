using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    List<Vector2> addedRooms = new List<Vector2>(){};
    [SerializeField] GameObject roomIcon;

    private void Awake() 
    {
        addedRooms.Add(new Vector2(0,0));
    }
    public void AddRoomToMap(Vector2 newLocation)
    {
        if(!addedRooms.Contains(newLocation))
        {
            addedRooms.Add(newLocation);
            Instantiate(roomIcon, new Vector3(newLocation.x + 12, newLocation.y + 9.5f, roomIcon.transform.position.z), Quaternion.identity, transform);
        }
        else
        {
            return;
        }
    }
}
