using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    [SerializeField] Blueprint blueprint;

    [SerializeField] ProjectileRepository projectiles;
    public void Build(List<Room> rooms, LevelManager level, Vector2 RoomSize)
    {
        System.DateTime before = System.DateTime.Now;

        foreach (Room room in rooms)
        {
            room.InstantiateDoors(blueprint, RoomSize);
        }
        ConnectDoors(rooms, RoomSize);
        CloseOpenDoors(rooms);
        PlaceItems(rooms, level);
        PlaceObstacles(rooms, RoomSize);
        Instantiate(blueprint.stairs, new Vector3(level.lastRoom.transform.position.x + RoomSize.x/2, level.lastRoom.transform.position.y + RoomSize.y/2, level.lastRoom.transform.position.z), Quaternion.identity, level.lastRoom.transform);
        System.DateTime after = System.DateTime.Now; 
        System.TimeSpan duration = after.Subtract(before);
        Debug.Log("Time to build rooms: " + duration.TotalMilliseconds + " milliseconds, which is: " + duration.TotalSeconds + " seconds");
    }

    void PlaceObstacles(List<Room> rooms, Vector2 RoomSize)
    {
        for(int i = 0; i < rooms.Count - 1; i++)
        {
            if(rooms[i].myItem is Key){
                Instantiate(blueprint.piedestal, new Vector3(rooms[i].transform.position.x + RoomSize.x/2, rooms[i].transform.position.y + RoomSize.y/2, rooms[i].transform.position.z), Quaternion.identity, rooms[i].transform);
                continue;};
            rooms[i].InstantiateObstacles(blueprint, Random.Range(0, 4));
        }
    }
    void ConnectDoors(List<Room> rooms, Vector2 RoomSize)
    {
        foreach(Room room in rooms)
        {
            foreach(GameObject door in room.doors)
            {
                foreach(Room connectingRoom in rooms)
                {
                    if(connectingRoom.transform.position == new Vector3(
                    room.transform.position.x + door.GetComponent<Door>().directionModifier.x * RoomSize.x,
                    room.transform.position.y + door.GetComponent<Door>().directionModifier.y * RoomSize.y,
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
        Key theKey = Instantiate(blueprint.key, new Vector3(chosenRoom.transform.position.x + level.roomSize.x/2, chosenRoom.transform.position.y + level.roomSize.y/2 + 1.5f, chosenRoom.transform.position.z), Quaternion.identity, chosenRoom.transform);
        chosenRoom.myItem = theKey;
        chosenRoom.myItem.gameObject.SetActive(false);
        roomsToChooseBetween.Remove(chosenRoom);

        List<PickUp> pickUpProbabilityList = new List<PickUp>(){};

        foreach(Blueprint.PickUpEntry pickup in blueprint.pickUps)
        {
            if(DebugManager.SpawnOnlyBalls && !pickup.item.gameObject.GetComponent<PowerUp>())
            {
                continue;
            }
            for(int i = 0; i < (int)pickup.rarity; i++)
            {
                pickUpProbabilityList.Add(pickup.item);
            }
        }

        foreach(Room room in roomsToChooseBetween)
        {
            PickUp newItem = Instantiate(pickUpProbabilityList[Random.Range(0, pickUpProbabilityList.Count)], new Vector3(room.transform.position.x + level.roomSize.x/2, room.transform.position.y + level.roomSize.y/2, room.transform.position.z), Quaternion.identity, room.transform);
            room.myItem = newItem;
            room.myItem.gameObject.SetActive(false);
            if(newItem.GetComponent<PowerUp>())
            {
                GameObject temp = Instantiate(blueprint.powerUpBalls[(int)newItem.GetComponent<PowerUp>().myType- 1], new Vector2(10000, 10000), Quaternion.identity, transform); 
                projectiles.balls.Add(temp.GetComponent<Ball>());
                temp.SetActive(false);
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
