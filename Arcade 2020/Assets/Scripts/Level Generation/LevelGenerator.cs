using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class LevelGenerator : MonoBehaviour
{
    List<Room> rooms = new List<Room>{};
    [SerializeField] Room RoomPrefab;
    int numberOfRooms = 1;
    private RoomBuilder builder;
    [Tooltip("X is for minimum value, Y is for maximum value")]
    [SerializeField] Vector2 MinMaxAmountOfRooms;
    [Tooltip("This controls the growth rate as the floors increase")]
    [SerializeField] float floorSizeMultiplier;

    void Awake() 
    {
        builder = GetComponent<RoomBuilder>();
    }
    public void GenerateLevel(LevelManager level, int currentFloor)
    {
        System.DateTime before = System.DateTime.Now;

        rooms.Add(Instantiate(RoomPrefab, Vector3.zero, Quaternion.identity, transform));
        rooms[0].Initialize();

        SpawnRooms(UnityEngine.Random.Range((int)(MinMaxAmountOfRooms.x + currentFloor * floorSizeMultiplier),
                                (int)(MinMaxAmountOfRooms.y + currentFloor * floorSizeMultiplier)));

        level.firstRoom = rooms[0];
        level.lastRoom = rooms[rooms.Count - 1];

        LockDoors(rooms[rooms.Count - 1]);
        builder.Build(rooms, level);

        System.DateTime after = System.DateTime.Now; 
        System.TimeSpan duration = after.Subtract(before);
        Debug.Log("Time to generate: " + duration.TotalMilliseconds + " milliseconds, which is: " + duration.TotalSeconds + " seconds");
    }

    void SpawnRooms(int amountOfRooms)
    {
        System.DateTime before = System.DateTime.Now;
        //this spawns all rooms
        for (int i = rooms.Count; i < amountOfRooms; i++)
        {
            Tuple<Room, List<RoomEntrance>> originRoom = GetRandomRoomInList();
            rooms.Add(Instantiate(RoomPrefab, transform));
            rooms[i].name = "Room #" + numberOfRooms; numberOfRooms++;

            rooms[i].Initialize(GetNewRoomCoordinates(originRoom.Item1.GetPosition(), originRoom.Item2));
            
            SetEntrances(originRoom.Item1, rooms[i]);
            LinkRoom(rooms[i]);
            OpenRandomEntrances(rooms[i]);
        }

        System.DateTime after = System.DateTime.Now; 
        System.TimeSpan duration = after.Subtract(before);
        Debug.Log("Time to spawn rooms: " + duration.TotalMilliseconds + " milliseconds, which is: " + duration.TotalSeconds + " seconds");
    }
    Tuple<Room, List<RoomEntrance>> GetRandomRoomInList()
    {
        //This functions gets any of the rooms that are already spawned
        //It should make sure that it doesnt have something spawned in each direction

        List<Tuple<Room, List<RoomEntrance>>> roomsWithOpenDoors = new List<Tuple<Room, List<RoomEntrance>>>{};

        foreach (Room room in rooms)
        {
            List<RoomEntrance> openEntrances = room.GetOpenUnspawnedEntrances();
            if (openEntrances.Count > 0)
            {
                roomsWithOpenDoors.Add(new Tuple<Room, List<RoomEntrance>>(room, openEntrances));
            }
        }
        return roomsWithOpenDoors[UnityEngine.Random.Range(0, roomsWithOpenDoors.Count - 1)];
    }
    Vector2 GetNewRoomCoordinates(Vector2 originCoordinates, List<RoomEntrance> openEntrances)
    {
        //This functions chooses one of the unoccupied directions around that room
        //When it does, it should change that rooms m_direction to say that the opposing entrance is both open and spawned
        //Debug.Log("Getting coordinates for new room");
        List<Vector2> possibleCoordinates = new List<Vector2> { };
        foreach(RoomEntrance entrance in openEntrances)
        {
            if(!CheckIfCoordinatesOccupied(new Vector2(originCoordinates.x + entrance.DirectionModifier.x * 20, originCoordinates.y + entrance.DirectionModifier.y * 20)))
            {
                possibleCoordinates.Add(new Vector2(originCoordinates.x + entrance.DirectionModifier.x * 20, originCoordinates.y + entrance.DirectionModifier.y * 20));
            }
        }
        return possibleCoordinates[UnityEngine.Random.Range(0, possibleCoordinates.Count - 1)];
    }
     void OpenRandomEntrances(Room room)
    {
        //This will open a random amount of doors in the newly spawned room
        List<RoomEntrance> possibleEntrancesToOpen = new List<RoomEntrance> { };

        foreach(RoomEntrance entrance in room.GetDirections().m_directions)
        {
            if(entrance.Open == false && entrance.Spawned == false)
            {
                possibleEntrancesToOpen.Add(entrance);
            }
        }
        if (possibleEntrancesToOpen.Count > 0)
        {
            for (uint i = (uint)UnityEngine.Random.Range(0, possibleEntrancesToOpen.Count-1); i < UnityEngine.Random.Range(4 - possibleEntrancesToOpen.Count, 5); i++)
            {
                possibleEntrancesToOpen[UnityEngine.Random.Range(0, possibleEntrancesToOpen.Count)].Open = true;
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

    bool CheckIfCoordinatesOccupied(Vector2 roomPosition)
    {
        foreach (Room room in rooms)
        {
            if(room.GetPosition() == roomPosition)
            {
                return true;
            }
        }
        return false;
    }

    void LockDoors(Room originRoom)
    {
        foreach(RoomEntrance entrance in originRoom.GetDirections().m_directions)
        {
            entrance.SetEntranceType(EntranceType.LockedDoor);
            foreach(Room room in rooms)
            {
                if(room.transform.position == new Vector3(
                    originRoom.transform.position.x + entrance.DirectionModifier.x * 20,
                    originRoom.transform.position.y + entrance.DirectionModifier.y * 20,
                    originRoom.transform.position.z))
                {
                    if(entrance.DirectionModifier.x != 0)
                    {
                        foreach(RoomEntrance otherEntrance in room.GetDirections().m_directions)
                        {
                            if(otherEntrance.DirectionModifier.x == entrance.DirectionModifier.x + otherEntrance.DirectionModifier.x * 2)
                            {
                                otherEntrance.SetEntranceType(EntranceType.LockedDoor);
                            }
                        }
                    }
                    else if(entrance.DirectionModifier.y != 0)
                    {
                        foreach(RoomEntrance otherEntrance in room.GetDirections().m_directions)
                        {
                            if(otherEntrance.DirectionModifier.y == entrance.DirectionModifier.y + otherEntrance.DirectionModifier.y * 2)
                            {
                                otherEntrance.SetEntranceType(EntranceType.LockedDoor);
                            }
                        }
                    }
                }
            }
        }
    }

    public void DestroyLevel()
    {
        for(int i = rooms.Count -1; i >= 0; i--)
        {
            Destroy(rooms[i].gameObject);
        }
        rooms.Clear();
        numberOfRooms = 1;
    }
}