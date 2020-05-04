using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    [SerializeField] Blueprint blueprint;
    [SerializeField] EntityManager entityManager;
    public void Build(List<Room> rooms, LevelManager level)
    {
        System.DateTime before = System.DateTime.Now;

        foreach (Room room in rooms)
        {
            room.InstantiateDoors(blueprint);
        }
        ConnectDoors(rooms);
        CloseOpenDoors(rooms);
        PlaceItems(rooms, level);
        Instantiate(blueprint.stairs, new Vector3(level.lastRoom.transform.position.x + 10, level.lastRoom.transform.position.y + 10, level.lastRoom.transform.position.z), Quaternion.identity, level.lastRoom.transform);
        System.DateTime after = System.DateTime.Now; 
        System.TimeSpan duration = after.Subtract(before);
        Debug.Log("Time to build rooms: " + duration.TotalMilliseconds + " milliseconds, which is: " + duration.TotalSeconds + " seconds");
    }
    void ConnectDoors(List<Room> rooms)
    {
        foreach(Room room in rooms)
        {
            foreach(GameObject door in room.doors)
            {
                foreach(Room connectingRoom in rooms)
                {
                    if(connectingRoom.transform.position == new Vector3(
                    room.transform.position.x + door.GetComponent<Door>().directionModifier.x * 20,
                    room.transform.position.y + door.GetComponent<Door>().directionModifier.y * 20,
                    room.transform.position.z))
                    {
                        if(door.GetComponent<Door>().directionModifier.x != 0)
                        {
                            foreach(GameObject otherDoor in connectingRoom.doors)
                            {
                                if(otherDoor.GetComponent<Door>().directionModifier.x == door.GetComponent<Door>().directionModifier.x + otherDoor.GetComponent<Door>().directionModifier.x * 2)
                                {
                                    door.GetComponent<Door>().otherDoor = otherDoor.GetComponent<Door>();
                                }
                            }
                        }
                        else if(door.GetComponent<Door>().directionModifier.y != 0)
                        {
                            foreach(GameObject otherDoor in connectingRoom.doors)
                            {
                                if(otherDoor.GetComponent<Door>().directionModifier.y == door.GetComponent<Door>().directionModifier.y + otherDoor.GetComponent<Door>().directionModifier.y * 2)
                                {
                                    door.GetComponent<Door>().otherDoor = otherDoor.GetComponent<Door>();
                                }
                            }
                        }
                    }
                
                }
            }
        }
    }
    void PlaceItems(List<Room> rooms, LevelManager level)
    {
        List<Room> roomsToChooseBetween = new List<Room>{};
        foreach(Room room in rooms)
        {
            if(room != level.firstRoom && room != level.lastRoom)
            {
                roomsToChooseBetween.Add(room);
            }
        }
        Room chosenRoom = roomsToChooseBetween[Random.Range(0, roomsToChooseBetween.Count)];
        Key theKey = Instantiate(blueprint.key, new Vector3(chosenRoom.transform.position.x + 10, chosenRoom.transform.position.y + 10, chosenRoom.transform.position.z), Quaternion.identity, chosenRoom.transform);
        chosenRoom.myItem = theKey;
        chosenRoom.myItem.gameObject.SetActive(false);
    }
    void CloseOpenDoors(List<Room> rooms)
    {
        //This function must close all open doors in each room that doesnt lead anywhere
        for(int i = 0; i < rooms.Count; i++)
        {
            if (!rooms[i].GetDirections())
            {
                continue;
            }
            for (int j = 0; j < rooms[i].GetDirections().m_directions.Count; j++)
            {
                if(rooms[i].GetDirections().m_directions[j] == null)
                {
                    continue;
                }
                if (!rooms[i].GetDirections().m_directions[j].Spawned || (!rooms[i].GetDirections().m_directions[j].Open && rooms[i].GetDirections().m_directions[j].Spawned))
                {
                   Destroy(rooms[i].GetDirections().m_directions[j].gameObject);
                   rooms[i].GetDirections().m_directions[j] = null;
                }
            }
        }
    }
}
