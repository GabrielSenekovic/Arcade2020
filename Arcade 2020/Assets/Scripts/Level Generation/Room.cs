using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
     List<List<WallPosition>> wallPositions = new List<List<WallPosition>> { };
    /*
     * 0 = top wall
     * 1 = left wall
     * 2 = right wall
     * 3 = bottom wall
     */
    [SerializeField] WallPosition wallPosition; // This will not be here later ofc
    RoomDirections directions;

    public bool IsBuilt;
    int stepsAwayFromMainRoom = 0;
    [SerializeField] Vector2Int CameraBoundaries;

    public List<GameObject> doors;
    public PickUp myItem = null;

    public void Initialize(Vector2 location)
    {
        CameraBoundaries = new Vector2Int(20, 20);
        directions = GetComponent<RoomDirections>();
        BuildWallArray();
        transform.position = location;
    }

    public void BuildWallArray()
    {
        for(int i = 0; i < CameraBoundaries.x; i++)
        {
            wallPositions.Add(new List<WallPosition>());
            for(int j = 0; j < CameraBoundaries.y; j++)
            {
                WallPosition temp = Instantiate(wallPosition, new Vector2(i, j), Quaternion.identity, transform);
                wallPositions[i].Add(temp);
                wallPositions[i][j].SetPosition(new Vector2(i,j));
            }
        }
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
    public int GetDistance()
    {
        return stepsAwayFromMainRoom;
    }
    public void SetDistance(int distance)
    {
        stepsAwayFromMainRoom = distance;
    }
    public void PlaceDownWalls()
    {
        PlaceDownWallFrame();
        DetermineWallVariant();
    }
    public void OnPlaceDownWallFrame(Vector2 entranceDirection, int limit, int startValue, 
    int x_modifier, int y_modifier, int x_offset, int y_offset, int height)
    {
        RoomEntrance temp = null;
        foreach (RoomEntrance entrance in directions.m_directions)
        {
            if (entrance.DirectionModifier == entranceDirection)
            {
                temp = entrance;
            }
        }
        if(temp == null){return;}
        for (int i = startValue; i < limit; i++)
        {
            if (i == 9 || i == 10) 
            {
                if (temp.Open == true)
                {
                    if (temp.Spawned == true)
                    {
                        continue;
                    }
                }
            }
            wallPositions[i * x_modifier + x_offset][i * y_modifier + y_offset].PlaceDown(height);
        }
    }
    public void PlaceDownWallFrame()
    {
        int roomWidth = CameraBoundaries.x;
        int roomHeigth = CameraBoundaries.y;

        if (!directions)
        {
            return;
        }
        OnPlaceDownWallFrame(new Vector2(0, -1), CameraBoundaries.x, 0, 1, 0, 0, 0, 2);
        OnPlaceDownWallFrame(new Vector2(-1, 0), CameraBoundaries.y, 1, 0, 1, 0, 0, 2);
        OnPlaceDownWallFrame(new Vector2(1, 0), CameraBoundaries.y, 1, 0, 1, CameraBoundaries.x - 1 ,0, 2);
        OnPlaceDownWallFrame(new Vector2(0, 1), CameraBoundaries.x - 1, 1, 1, 0, 0 ,CameraBoundaries.y - 1, 2);

        OnPlaceDownWallFrame(new Vector2(0, -1), CameraBoundaries.x - 1, 1, 1, 0, 0, 1, 1);
        OnPlaceDownWallFrame(new Vector2(-1, 0), CameraBoundaries.y - 1, 2, 0, 1, 1, 0, 1);
        OnPlaceDownWallFrame(new Vector2(1, 0), CameraBoundaries.y - 1, 2, 0, 1, CameraBoundaries.x - 2 ,0, 1);
        OnPlaceDownWallFrame(new Vector2(0, 1), CameraBoundaries.x - 2, 2, 1, 0, 0 ,CameraBoundaries.y - 2, 1);
    }
    public void DetermineWallVariant()
    {
        for(int i = 0; i < wallPositions.Count; i++)
        {
            for(int j = 0; j < wallPositions[i].Count; j++)
            {
                string temp = "";
                if (wallPositions[i][j].GetIsOccupied())
                {
                    if(j+1 < CameraBoundaries.y)
                    {
                        if (wallPositions[i][j + 1].GetIsOccupied())
                        {
                            temp+='A';
                        }
                        else
                        {
                            temp+='B';
                        }
                    }
                    else
                    {
                        temp+='B';
                    }
                    if (i + 1 < CameraBoundaries.x)
                    {
                        if (wallPositions[i+1][j].GetIsOccupied())
                        {
                            temp+='A';
                        }
                        else
                        {
                            temp+='B';
                        }
                    }
                    else
                    {
                        temp+='B';
                    }
                    if (j - 1 >= 0)
                    {
                        if (wallPositions[i][j - 1].GetIsOccupied())
                        {
                            temp+='A';
                        }
                        else
                        {
                            temp+='B';
                        }
                    }
                    else
                    {
                        temp+='B';
                    }
                    if (i - 1 >= 0)
                    {
                        if (wallPositions[i-1][j].GetIsOccupied())
                        {
                            temp+='A';
                        }
                        else
                        {
                            temp+='B';
                        }
                    }
                    else
                    {
                        temp+='B';
                    }
                }
                if(wallPositions[i][j].heightLevel == 1)
                {
                    switch (temp)
                    {
                        case "AAAB":
                            wallPositions[i][j].SetVariant(WallVariant.Right_1);
                            break;
                        case "AABA":
                            wallPositions[i][j].SetVariant(WallVariant.Top_1);
                            break;
                        case "AABB":
                            wallPositions[i][j].SetVariant(WallVariant.BottomLeft_1);
                            break; 
                        case "ABAA":
                            wallPositions[i][j].SetVariant(WallVariant.Left_1);
                            break;
                        case "ABBA":
                            wallPositions[i][j].SetVariant(WallVariant.BottomRight_1);
                            break;
                        case "BAAA":
                            wallPositions[i][j].SetVariant(WallVariant.Bottom_1);
                            break;
                        case "BAAB":
                            wallPositions[i][j].SetVariant(WallVariant.TopLeft_1);
                            break;
                        case "BBAA":
                            wallPositions[i][j].SetVariant(WallVariant.TopRight_1);
                            break;
                        default: break;
                    }
                }
                if(wallPositions[i][j].heightLevel == 2)
                {
                    switch (temp)
                    {
                        case "AAAB":
                            wallPositions[i][j].SetVariant(WallVariant.Left_2);
                            break;
                        case "AABA":
                            wallPositions[i][j].SetVariant(WallVariant.Bottom_2);
                            break;
                        case "AABB":
                            wallPositions[i][j].SetVariant(WallVariant.BottomLeft_2);
                            break; 
                        case "ABAA":
                            wallPositions[i][j].SetVariant(WallVariant.Right_2);
                            break;
                        case "ABBA":
                            wallPositions[i][j].SetVariant(WallVariant.BottomRight_2);
                            break;
                        case "BAAA":
                            wallPositions[i][j].SetVariant(WallVariant.Top_2);
                            break;
                        case "BAAB":
                            wallPositions[i][j].SetVariant(WallVariant.TopLeft_2);
                            break;
                        case "BBAA":
                            wallPositions[i][j].SetVariant(WallVariant.TopRight_2);
                            break;
                        default: break;
                    }
                }
            }
        }

    }
    public void InstantiateWalls(WallBlueprints blueprints)
    {
        GameObject wallParent = new GameObject("Walls");
        wallParent.transform.SetParent(transform);
        for (int i = 0; i < CameraBoundaries.x; i++)
        {
            for (int j = 0; j < CameraBoundaries.y; j++)
            {
                if (wallPositions[i][j].GetIsOccupied())
                {
                    //GameObject newWall = Instantiate(blueprints.wallBlock, new Vector2(transform.position.x, transform.position.y) + wallPositions[i][j].GetPosition(), Quaternion.identity, transform);
                    if (wallPositions[i][j].GetVariant() != WallVariant.None)
                    {
                        Wall newWall = Instantiate(blueprints.GetWall(wallPositions[i][j].GetVariant()), new Vector2(transform.position.x, transform.position.y) + wallPositions[i][j].GetPosition(), Quaternion.identity, wallParent.transform);

                        //for(int k = 0; k < newWall.GetAmountOfRenderers(); k++)
                        //{
                        //   newWall.ChangeColor(GetWallColor(), k);
                        //}
                    }
                }
            }
        }
    }
    public void InstantiateFloor(GameObject floorTile)
    {
        GameObject floorParent = new GameObject("Floor");
        floorParent.transform.SetParent(transform);
        for (int i = 0; i < CameraBoundaries.x; i++)
        {
            for (int j = 0; j < CameraBoundaries.y; j++)
            {
                if (!wallPositions[i][j].GetIsOccupied())
                {
                    GameObject newWall = Instantiate(floorTile, new Vector2(transform.position.x, transform.position.y) + wallPositions[i][j].GetPosition(), Quaternion.identity, floorParent.transform);
                }
            }
        }
    }
    public void InstantiateDoors(WallBlueprints blueprints)
    {
        if(directions.m_directions[0].Open && directions.m_directions[0].Spawned)
        {
            GameObject door = Instantiate(blueprints.door, new Vector2(transform.position.x + 9, transform.position.y + 19), Quaternion.identity, transform);
            door.GetComponent<Door>().directionModifier = directions.m_directions[0].DirectionModifier;
            if(directions.m_directions[0].GetEntranceType() == EntranceType.LockedDoor)
            {
                door.GetComponent<Door>().Lock();
            }
            doors.Add(door);
        }
        if(directions.m_directions[1].Open && directions.m_directions[1].Spawned)
        {
            GameObject door = Instantiate(blueprints.door, new Vector2(transform.position.x + 18, transform.position.y + 10), Quaternion.identity, transform);
            door.GetComponentInChildren<SpriteRenderer>().transform.Rotate(new Vector3(0, 0,270), Space.Self);
            door.GetComponent<Door>().directionModifier = directions.m_directions[1].DirectionModifier;
            if(directions.m_directions[1].GetEntranceType() == EntranceType.LockedDoor)
            {
                door.GetComponent<Door>().Lock();
            }
            doors.Add(door);
        }
        if(directions.m_directions[2].Open && directions.m_directions[2].Spawned)
        {
            GameObject door = Instantiate(blueprints.door, new Vector2(transform.position.x, transform.position.y + 10), Quaternion.identity, transform);
            door.GetComponentInChildren<SpriteRenderer>().transform.Rotate(new Vector3(0, 0, 90), Space.Self);
            door.GetComponent<Door>().directionModifier = directions.m_directions[2].DirectionModifier;
            if(directions.m_directions[2].GetEntranceType() == EntranceType.LockedDoor)
            {
                door.GetComponent<Door>().Lock();
            }
            doors.Add(door);
        }
        if(directions.m_directions[3].Open && directions.m_directions[3].Spawned)
        {
            GameObject door = Instantiate(blueprints.door, new Vector2(transform.position.x + 9, transform.position.y + 1), Quaternion.identity, transform);
            door.GetComponentInChildren<SpriteRenderer>().transform.Rotate(new Vector3(0, 0, 180), Space.Self);
            door.GetComponent<Door>().directionModifier = directions.m_directions[3].DirectionModifier;
            if(directions.m_directions[3].GetEntranceType() == EntranceType.LockedDoor)
            {
                door.GetComponent<Door>().Lock();
            }
            doors.Add(door);
        }
    }
}