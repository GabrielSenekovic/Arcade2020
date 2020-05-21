using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    RoomDirections directions;
    Vector2 CameraBoundaries;

    public class ObstacleEntry
    {
        public ObstacleEntry(Vector2 newLocation, bool value)
        {
            location = newLocation;
            occupied = value;
        }
        public Vector2 location;
        public bool occupied;
    }
    public List<List<ObstacleEntry>> ObstacleLocations = new List<List<ObstacleEntry>>(){};

    public List<GameObject> doors;
    public PickUp myItem = null;

    public bool roomCleared = false;

    public void Initialize(Vector2 RoomSize)
    {
        //This Initialize() function is for the origin room specifically, as it already has its own position
        roomCleared = true;
        OpenAllEntrances();
        CameraBoundaries = RoomSize;
        directions = GetComponent<RoomDirections>();
        AdjustColliders(RoomSize);
    }

    public void Initialize(Vector2 location, Vector2 RoomSize)
    {
        CameraBoundaries = RoomSize;
        directions = GetComponent<RoomDirections>();
        transform.position = location;
        AdjustColliders(RoomSize);
        AddLocations(RoomSize);
    }

    public void AddLocations(Vector2 RoomSize)
    {
        for(int i = 5; i < RoomSize.x - 5; i++)
        {
            ObstacleLocations.Add(new List<ObstacleEntry>(){});
            for(int j = 5; j < RoomSize.y - 5; j++)
            {
                ObstacleLocations[i-5].Add(new ObstacleEntry(new Vector2(i,j), false));
            }
        }
        for(int i = 0; i > 3; i++)
        {
            ObstacleLocations[(int)RoomSize.x/2 + i][4].occupied = true;
            ObstacleLocations[(int)RoomSize.x/2 + i][4 + i].occupied = true;
            ObstacleLocations[(int)RoomSize.x/2 + i][(int)RoomSize.y - 4].occupied = true;
            ObstacleLocations[(int)RoomSize.x/2 + i][(int)RoomSize.y - 4 - i].occupied = true;
            ObstacleLocations[4][(int)RoomSize.y/2 + i].occupied = true;
            ObstacleLocations[4 + i][(int)RoomSize.y/2 + i].occupied = true;
            ObstacleLocations[(int)RoomSize.x - 4][(int)RoomSize.y/2 + i].occupied = true;
            ObstacleLocations[(int)RoomSize.x - 4 - i][(int)RoomSize.y/2 + i].occupied = true;
        }
    }
    public void AdjustColliders(Vector2 RoomSize)
    {
        BoxCollider2D[] colliders = GetComponentsInChildren<BoxCollider2D>();
        colliders[0].offset = new Vector2(0.075f* RoomSize.x, 0.475f*RoomSize.y);
        colliders[0].size = new Vector2(1, RoomSize.y *0.85f);
        colliders[1].offset = new Vector2(0.925f * RoomSize.x, 0.475f*RoomSize.y);
        colliders[1].size = new Vector2(1, RoomSize.y*0.85f);
        colliders[2].offset = new Vector2(0.5f *RoomSize.x, 0.075f*RoomSize.y);
        colliders[2].size = new Vector2(RoomSize.x*0.8f, 1);
        colliders[3].offset = new Vector2(0.5f* RoomSize.x, 0.875f*RoomSize.y);
        colliders[3].size = new Vector2(RoomSize.x*0.8f, 1);
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

    public void InstantiateDoors(Blueprint blueprints, Vector2 RoomSize)
    {
        if(directions.m_directions[0].Open && directions.m_directions[0].Spawned)
        {
            OnInstantiateDoor(blueprints, 0, 0.5f * RoomSize.x, 0.9f * RoomSize.y, 0);
        }
        if(directions.m_directions[1].Open && directions.m_directions[1].Spawned)
        {
            OnInstantiateDoor(blueprints, 1, 0.95f*RoomSize.x, 0.475f * RoomSize.y, 270);
        }
        if(directions.m_directions[2].Open && directions.m_directions[2].Spawned)
        {
            OnInstantiateDoor(blueprints, 2, 0.05f * RoomSize.x, 0.475f * RoomSize.y, 90);
        }
        if(directions.m_directions[3].Open && directions.m_directions[3].Spawned)
        {
            OnInstantiateDoor(blueprints, 3, 0.5f * RoomSize.x, 0.05f * RoomSize.y, 180);
        }
    }
    void OnInstantiateDoor(Blueprint blueprints, int i, float Xoffset, float Yoffset, int rotation)
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

    public void InstantiateObstacles(Blueprint blueprints, int amount)
    {
        if(ObstacleLocations.Count == 0)
        {
            return;
        }
        for(int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, blueprints.obstacles.Count);
            int Xindex = Random.Range(0, ObstacleLocations.Count);
            Debug.Log("X: " + Xindex);
            int Yindex = Random.Range(0, ObstacleLocations[Xindex].Count);
            Debug.Log("Y: " + Yindex);
            Vector2 location = ObstacleLocations[Xindex][Yindex].location;
            for(int j = 0; j < blueprints.obstacles[index].sizeToTakeUp.x; j++)
            {
                if(Xindex + j >= ObstacleLocations.Count){goto End;}
                for(int k = 0; k < blueprints.obstacles[index].sizeToTakeUp.y; k++)
                {
                    if(Yindex - k < 0){goto End;}
                    if(ObstacleLocations[Xindex + j][Yindex - k].occupied == true)
                    {
                        Debug.Log("Going to end");
                        goto End;
                    }
                    else
                    {
                        ObstacleLocations[Xindex+j][Yindex-k].occupied = true;
                    }
                }
            }
                
            Debug.Log("Time to instantiate");
            Instantiate(blueprints.obstacles[index], location + (Vector2)transform.position, Quaternion.identity, transform);
            End:;
        }
    }
}