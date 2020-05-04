using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    RoomDirections directions;
    [SerializeField] Vector2Int CameraBoundaries;

    public List<GameObject> doors;
    public PickUp myItem = null;

    public bool roomCleared = false;

    public void Initialize()
    {
        //This Initialize() function is for the origin room specifically, as it already has its own position
        roomCleared = true;
        OpenAllEntrances();
        CameraBoundaries = new Vector2Int(20, 20);
        directions = GetComponent<RoomDirections>();
    }

    public void Initialize(Vector2 location)
    {
        CameraBoundaries = new Vector2Int(20, 20);
        directions = GetComponent<RoomDirections>();
        transform.position = location;
    }
    public List<RoomEntrance> GetOpenUnspawnedEntrances()
    {
        List<RoomEntrance> openEntrances = new List<RoomEntrance>{};
        foreach(RoomEntrance entrance in directions.m_directions)
        {
            if (entrance.Open && !entrance.Spawned)
            {
                openEntrances.Add(entrance);
            }
        }
        return openEntrances;
    }
    public void OpenAllEntrances()
    {
        if(!directions)
        {
            directions = GetComponent<RoomDirections>();
        }
        directions.OpenAllEntrances();
    }
    public Vector2 GetPosition()
    {
        return transform.position;
    }
    public RoomDirections GetDirections()
    {
        return directions;
    }
    public void RevealItem()
    {
        if(myItem)
        {
            myItem.gameObject.SetActive(true);
        }
        FindObjectOfType<AudioManager>().Play("Door");
    }

    public void InstantiateDoors(Blueprint blueprints)
    {
        if(directions.m_directions[0].Open && directions.m_directions[0].Spawned)
        {
            OnInstantiateDoor(blueprints, 0, 10, 18, 0);
        }
        if(directions.m_directions[1].Open && directions.m_directions[1].Spawned)
        {
            OnInstantiateDoor(blueprints, 1, 19, 9.5f, 270);
        }
        if(directions.m_directions[2].Open && directions.m_directions[2].Spawned)
        {
            OnInstantiateDoor(blueprints, 2, 1, 9.5f, 90);
        }
        if(directions.m_directions[3].Open && directions.m_directions[3].Spawned)
        {
            OnInstantiateDoor(blueprints, 3, 10, 1, 180);
        }
    }
    void OnInstantiateDoor(Blueprint blueprints, int i, int Xoffset, float Yoffset, int rotation)
    {
        GameObject door = Instantiate(blueprints.door, new Vector2(transform.position.x + Xoffset, transform.position.y + Yoffset), Quaternion.identity, transform);
        door.transform.Rotate(new Vector3(0, 0, rotation), Space.Self);
        door.GetComponent<Door>().directionModifier = directions.m_directions[i].DirectionModifier;
        if(directions.m_directions[i].GetEntranceType() == EntranceType.LockedDoor)
        {
            door.GetComponent<Door>().Lock();
        }
        doors.Add(door);
    }
}