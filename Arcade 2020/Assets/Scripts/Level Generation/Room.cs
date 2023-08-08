using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    RoomDirections directions;
    Vector2 CameraBoundaries;

    [System.Serializable]
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
    public List<ObstacleEntry> ObstacleLocations = new List<ObstacleEntry>(){};

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
        int width = (int)RoomSize.x - 8;
        for (int j = (int)RoomSize.y-3; j >= 5; j -= 2)
        {
            for (int i = 4; i <= RoomSize.x - 4; i += 2)
            {
                ObstacleLocations.Add(new ObstacleEntry(new Vector2(i, j), false));
            }
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
        if(ObstacleLocations.Count > 0)
        {
            float yPosition = Mathf.Ceil(7 -(Yoffset / 2 + 5)/2);
            int xPosition = (int)((Xoffset-4- directions.m_directions[i].DirectionModifier.x) / 2);
            int obstacleIndex = (int)(yPosition * 11) + xPosition;
            ObstacleLocations[obstacleIndex].occupied = true;
        }
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
            int Xindex = Random.Range(0, 11);
            int Yindex = Random.Range(0, 7);
            ObstacleEntry temp = ObstacleLocations[Yindex * 11 + Xindex];
            if(!temp.occupied)
            {
                Vector2 location = temp.location;
                Instantiate(blueprints.obstacles[index], location + (Vector2)transform.position, Quaternion.identity, transform);
            }
        }
    }
}