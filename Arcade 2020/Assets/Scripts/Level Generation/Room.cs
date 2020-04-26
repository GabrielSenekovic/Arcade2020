using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    RoomDirections directions;
    [SerializeField] Vector2Int CameraBoundaries;

    public List<GameObject> doors;
    public PickUp myItem = null;

    public void Initialize()
    {
        //This Initialize() function is for the origin room specifically, as it already has its own position
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
    public bool GetIfHasOneOpenEntrance()
    {
        if(directions == null)
        {
            return false;
        }
        foreach(RoomEntrance entrance in directions.m_directions)
        {
            if(entrance == null)
            {
                continue;
            }
            if (entrance.Open && !entrance.Spawned)
            {
                return true;
            }
        }
        return false;
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

    public void InstantiateDoors(Blueprint blueprints)
    {
        if(directions.m_directions[0].Open && directions.m_directions[0].Spawned)
        {
            OnInstantiateDoor(blueprints, 0, 9, 19, 0);
        }
        if(directions.m_directions[1].Open && directions.m_directions[1].Spawned)
        {
            OnInstantiateDoor(blueprints, 1, 18, 10, 270);
        }
        if(directions.m_directions[2].Open && directions.m_directions[2].Spawned)
        {
            OnInstantiateDoor(blueprints, 2, 0, 10, 90);
        }
        if(directions.m_directions[3].Open && directions.m_directions[3].Spawned)
        {
            OnInstantiateDoor(blueprints, 3, 9, 1, 180);
        }
    }
    void OnInstantiateDoor(Blueprint blueprints, int i, int Xoffset, int Yoffset, int rotation)
    {
        GameObject door = Instantiate(blueprints.door, new Vector2(transform.position.x + Xoffset, transform.position.y + Yoffset), Quaternion.identity, transform);
        door.GetComponentInChildren<SpriteRenderer>().transform.Rotate(new Vector3(0, 0, rotation), Space.Self);
        door.GetComponent<Door>().directionModifier = directions.m_directions[i].DirectionModifier;
        if(directions.m_directions[i].GetEntranceType() == EntranceType.LockedDoor)
        {
            door.GetComponent<Door>().Lock();
        }
        doors.Add(door);
    }
}