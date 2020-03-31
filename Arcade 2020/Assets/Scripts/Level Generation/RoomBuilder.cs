using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    [SerializeField] WallBlueprints WallBlockPrefab;
    [SerializeField] GameObject FloorTilePrefab;
    public void Build(List<Room> rooms)
    {
        BuildRooms(rooms);
        CloseOpenDoors(rooms);
    }
    void BuildRooms(List<Room> rooms)
    {
        foreach (Room room in rooms)
        {
            if (!room.IsBuilt)
            {
                room.PlaceDownWalls();
                room.InstantiateWalls(WallBlockPrefab);
                room.InstantiateFloor(FloorTilePrefab);

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
