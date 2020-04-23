﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    [SerializeField] WallBlueprints WallBlockPrefab;
    [SerializeField] GameObject FloorTilePrefab;
    public void Build(List<Room> rooms, LevelManager level)
    {
        BuildRooms(rooms, level);
        ConnectDoors(rooms);
        CloseOpenDoors(rooms);
        PlaceItems(rooms, level);
    }
    void BuildRooms(List<Room> rooms, LevelManager level)
    {
        foreach (Room room in rooms)
        {
            if (!room.IsBuilt)
            {
                room.PlaceDownWalls();
                room.InstantiateWalls(WallBlockPrefab);
                room.InstantiateFloor(FloorTilePrefab);
                room.InstantiateDoors(WallBlockPrefab);
                Instantiate(WallBlockPrefab.stairs, new Vector3(level.lastRoom.transform.position.x + 10, level.lastRoom.transform.position.y + 10, level.lastRoom.transform.position.z), Quaternion.identity, level.lastRoom.transform);

                foreach (Transform child in room.transform)
                {
                    if (child.GetComponent<WallPosition>())
                    {
                        Destroy(child.gameObject);
                    }
                }
                room.IsBuilt = true;
            }
        }
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
        Key theKey = Instantiate(WallBlockPrefab.key, new Vector3(chosenRoom.transform.position.x + 10, chosenRoom.transform.position.y + 10, chosenRoom.transform.position.z), Quaternion.identity, chosenRoom.transform);
        chosenRoom.myItem = theKey;
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
