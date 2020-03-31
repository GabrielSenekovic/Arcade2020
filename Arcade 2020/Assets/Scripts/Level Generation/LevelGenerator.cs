using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    List<Room> rooms = new List<Room>{};
    Room RoomPrefab;
    int numberOfRooms;
    int furthestDistanceFromSpawn;
    private RoomBuilder builder;
    void Initiate(Room originRoom)
    {
        //originRoom.OpenAllEntrances(); originRoom.Initialize(originRoom.transform.position);
        //SpawnRooms(Random.Range((int)m_data.GetRoomAmountCap().x + rooms.Count, (int)m_data.GetRoomAmountCap().y + rooms.Count));
        SpawnRooms(Random.Range(10, 20));
        //AdjustEntrances();
        builder.Build(rooms);
    }
    void SpawnRooms(int amountOfRooms)
    {
        //this spawns all rooms
        for (int i = rooms.Count; i < amountOfRooms; i++)
        {
            Room originRoom = GetRandomRoomInList();
            rooms.Add(Instantiate(RoomPrefab, transform));
            //Debug.Log(i);
            rooms[i].name = "Room #" + numberOfRooms; numberOfRooms++;
            Vector2 newCoordinates = GetNewRoomCoordinates(originRoom.GetPosition(), originRoom.GetDirections());
            while(true)
            {
                if(newCoordinates != new Vector2(0,0))
                {
                   // rooms[i].Initialize(newCoordinates);
                    break;
                }
                else
                {
                    originRoom = GetRandomRoomInList();
                    newCoordinates = GetNewRoomCoordinates(originRoom.GetPosition(), originRoom.GetDirections());
                }
            }
            /*rooms[i].SetDistance(originRoom.GetDistance() + 1);
            if(rooms[i].GetDistance() > furthestDistanceFromSpawn)
            {
                furthestDistanceFromSpawn = rooms[i].GetDistance();
            }*/
            SetEntrances(originRoom, rooms[i]);
            LinkRoom(rooms[i]);
            OpenRandomEntrances(rooms[i]);
        }
    }
    Room GetRandomRoomInList()
    {
        //This functions gets any of the rooms that are already spawned
        //It should make sure that it doesnt have something spawned in each direction
        //Debug.Log("Getting the coordinates of a random spawned room");
        if (rooms.Count != 0)
        {
            List<Room> roomWithOpenDoors = new List<Room> { };
            foreach (Room room in rooms)
            {
                /*if (room.GetIfHasOneOpenEntrance())
                {
                    roomWithOpenDoors.Add(room);
                }*/
            }
            return roomWithOpenDoors[Random.Range(0, roomWithOpenDoors.Count - 1)];
        }
        return rooms[0];
    }
    Vector2 GetNewRoomCoordinates(Vector2 originCoordinates, RoomDirections directionsOfRoom)
    {
        //This functions chooses one of the unoccupied directions around that room
        //When it does, it should change that rooms m_direction to say that the opposing entrance is both open and spawned
        //Debug.Log("Getting coordinates for new room");
        List<Vector2> possibleCoordinates = new List<Vector2> { };
        for(int i = 0; i < 4; i++)
        {
            if (directionsOfRoom.m_directions[i].Open && !directionsOfRoom.m_directions[i].Spawned)
            {
                /*if(!CheckIfCoordinatesOccupied(new Vector2(originCoordinates.x + directionsOfRoom.m_directions[i].DirectionModifier.x * 20, originCoordinates.y + directionsOfRoom.m_directions[i].DirectionModifier.y * 20)))
                {
                    possibleCoordinates.Add(new Vector2(originCoordinates.x + directionsOfRoom.m_directions[i].DirectionModifier.x * 20, originCoordinates.y + directionsOfRoom.m_directions[i].DirectionModifier.y * 20));
                }*/
            }
        }
        int index = Random.Range(0, possibleCoordinates.Count - 1);
        if(possibleCoordinates.Count > 0)
        {
            return possibleCoordinates[index];
        }
        else
        {
            return new Vector2(0,0);
        }
    }
     void OpenRandomEntrances(Room room)
    {
        List<RoomEntrance> possibleEntrancesToOpen = new List<RoomEntrance> { };
        if(!room.GetDirections())
        {
            return;
        }
        foreach(RoomEntrance entrance in room.GetDirections().m_directions)
        {
            if(entrance.Open == false && entrance.Spawned == false)
            {
                possibleEntrancesToOpen.Add(entrance);
            }
        }
        if (possibleEntrancesToOpen.Count != 0)
        {
            for (int i = 0; i < Random.Range(4 - possibleEntrancesToOpen.Count, 5); i++)
            {
                possibleEntrancesToOpen[Random.Range(0, possibleEntrancesToOpen.Count)].Open = true;
            }
        }
    }
    void LinkRoom(Room room)
    {
        //This function checks if this given room has another spawned room in any direction that it must link to, before it decides if it should link anywhere else
        //It does this by checking if a room in any direction has an open but not spawned gate in its own direction, in which case it opens its own gate in that direction
        for (int i = 0; i < rooms.Count; i++)
        {
            if(rooms[i].GetPosition() == new Vector2(room.GetPosition().x + 20, room.GetPosition().y))
            {
                if (rooms[i].GetDirections().m_directions[2] == null)
                {
                    continue;
                }
                if (rooms[i].GetDirections().m_directions[2].Open)
                {
                    SetEntrances(room, rooms[i]);
                    if (rooms[i].GetDistance() < room.GetDistance() - 1)
                    {
                        room.SetDistance(rooms[i].GetDistance() + 1);
                    }
                }
                else
                {
                    room.GetDirections().m_directions[1].Open = false;
                    room.GetDirections().m_directions[1].Spawned = true;
                }
            }
            else if (rooms[i].GetPosition() == new Vector2(room.GetPosition().x - 20, room.GetPosition().y))
            {
                if(rooms[i].GetDirections().m_directions[1] == null)
                {
                    continue;
                }
                if (rooms[i].GetDirections().m_directions[1].Open)
                {
                    SetEntrances(room, rooms[i]);
                    if (rooms[i].GetDistance() < room.GetDistance() - 1)
                    {
                        room.SetDistance(rooms[i].GetDistance() + 1);
                    }
                }
                else
                {
                    room.GetDirections().m_directions[2].Open = false;
                    room.GetDirections().m_directions[2].Spawned = true;
                }
            }
            else if (rooms[i].GetPosition() == new Vector2(room.GetPosition().x, room.GetPosition().y + 20))
            {
                if (rooms[i].GetDirections().m_directions[3] == null)
                {
                    continue;
                }
                if (rooms[i].GetDirections().m_directions[3].Open)
                {
                    SetEntrances(room, rooms[i]);
                    if (rooms[i].GetDistance() < room.GetDistance() - 1)
                    {
                        room.SetDistance(rooms[i].GetDistance() + 1);
                    }
                }
                else
                {
                    room.GetDirections().m_directions[0].Open = false;
                    room.GetDirections().m_directions[0].Spawned = true;
                }
            }
            else if (rooms[i].GetPosition() == new Vector2(room.GetPosition().x, room.GetPosition().y - 20))
            {
                if(rooms[i].GetDirections().m_directions[0] == null)
                {
                    continue;
                }
                if (rooms[i].GetDirections().m_directions[0].Open)
                {
                    SetEntrances(room, rooms[i]);
                    if (rooms[i].GetDistance() < room.GetDistance() - 1)
                    {
                        room.SetDistance(rooms[i].GetDistance() + 1);
                    }
                }
                else
                {
                    room.GetDirections().m_directions[3].Open = false;
                    room.GetDirections().m_directions[3].Spawned = true;
                }
            }
        }
    }
    void SetEntrances(Room RoomA, Room RoomB)
    {
        if (RoomA.GetPosition().x > RoomB.GetPosition().x)
        {
            //New room is to the right?
            if(RoomA.GetDirections())
            {
                RoomA.GetDirections().m_directions[2].Open = true;
                RoomA.GetDirections().m_directions[2].Spawned = true;
            }
            if(RoomB.GetDirections())
            {
                RoomB.GetDirections().m_directions[1].Open = true;
                RoomB.GetDirections().m_directions[1].Spawned = true;
            }
        }
        else if(RoomA.GetPosition().x < RoomB.GetPosition().x)
        {
            //New room is to the left?
            if(RoomA.GetDirections())
            {
                RoomA.GetDirections().m_directions[1].Open = true;
                RoomA.GetDirections().m_directions[1].Spawned = true;
            }
            if(RoomB.GetDirections())
            {
                RoomB.GetDirections().m_directions[2].Open = true;
                RoomB.GetDirections().m_directions[2].Spawned = true;
            }
        }
else
        {
            if(RoomA.GetPosition().y > RoomB.GetPosition().y)
            {
                //New room is down
                if(RoomA.GetDirections())
                {
                    RoomA.GetDirections().m_directions[3].Open = true;
                    RoomA.GetDirections().m_directions[3].Spawned = true;
                }
                if(RoomB.GetDirections())
                {
                    RoomB.GetDirections().m_directions[0].Open = true;
                    RoomB.GetDirections().m_directions[0].Spawned = true;
                }
            }
            else if (RoomA.GetPosition().y < RoomB.GetPosition().y)
            {
                //New room is up
                if(RoomA.GetDirections())
                {
                    RoomA.GetDirections().m_directions[0].Open = true;
                    RoomA.GetDirections().m_directions[0].Spawned = true;
                }
                if(RoomB.GetDirections())
                {
                    RoomB.GetDirections().m_directions[3].Open = true;
                    RoomB.GetDirections().m_directions[3].Spawned = true;
                }
            }
            else
            {
                Debug.LogWarning("These are either the same room, or on top of eachother!");
            }
        }
    }
}