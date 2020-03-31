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
    public void PlaceDownWallFrame()
    {
        int roomWidth = CameraBoundaries.x;
        int roomHeigth = CameraBoundaries.y;
        int j = 0;

        List<RoomEntrance> entrancesToTheSouth = new List<RoomEntrance> { };

        if (!directions)
        {
            return;
        }
        foreach (RoomEntrance entrance in directions.m_directions)
        {
            if (entrance.DirectionModifier == new Vector2(0, -1))
            {
                entrancesToTheSouth.Add(entrance);
            }
        }

        for (int i = 0; i < roomWidth; i++)
        {
            if (i == j * 20 + 9) 
            {
                entrancesToTheSouth[j].gameObject.transform.position = new Vector2(transform.position.x + i, transform.position.y);
                if (entrancesToTheSouth[j].Open == true)
                {
                    if (entrancesToTheSouth[j].Spawned == true)
                    {
                        continue;
                    }
                }
            }
            if (i == 10 + j * 20)
            {
                j++;
                if (entrancesToTheSouth[j - 1].Open == true)
                {
                    if (entrancesToTheSouth[j - 1].Spawned == true)
                    {
                        continue;
                    }
                }
            }
            wallPositions[i][0].PlaceDown(new Vector2(transform.position.x + i, transform.position.y));
        }
        j = 0;
 List<RoomEntrance> entrancesToTheRight = new List<RoomEntrance> { };

        foreach (RoomEntrance entrance in directions.m_directions)
        {
            if (entrance.DirectionModifier == new Vector2(-1, 0))
            {
                entrancesToTheRight.Add(entrance);
            }
        }
        for (int i = 1; i < roomHeigth; i++)
        {
            if (i == 9 + j * 20)
            {
                entrancesToTheRight[j].gameObject.transform.position = new Vector2(transform.position.x, transform.position.y + i);
                if (entrancesToTheRight[j].Open == true)
                {
                    if (entrancesToTheRight[j].Spawned == true)
                    {
                        continue;
                    }
                }
            }
            if (i == 10 + j * 20)
            {
                j++;
                if (entrancesToTheRight[j - 1].Open == true)
                {
                    if (entrancesToTheRight[j - 1].Spawned == true)
                    {
                        continue;
                    }
                }
            }
            wallPositions[0][i].PlaceDown(new Vector2(transform.position.x, transform.position.y + i));
        }
        j = 0;
List<RoomEntrance> entrancesToTheLeft = new List<RoomEntrance> { };

        foreach (RoomEntrance entrance in directions.m_directions)
        {
            if (entrance.DirectionModifier == new Vector2(1, 0))
            {
                entrancesToTheLeft.Add(entrance);
            }
        }
        for (int i = 1; i < roomHeigth; i++)
        {
            if (i == 9 + j * 20)
            {
                entrancesToTheLeft[j].gameObject.transform.position = new Vector2(transform.position.x + roomWidth - 1, transform.position.y + i);
                if (entrancesToTheLeft[j].Open == true)
                {
                    if (entrancesToTheLeft[j].Spawned == true)
                    {
                        continue;
                    }
                }
            }
            if (i == 10 + j * 20)
            {
                j++;
                if (entrancesToTheLeft[j - 1].Open == true)
                {
                    if (entrancesToTheLeft[j - 1].Spawned == true)
                    {
                        continue;
                    }
                }
            }
            //if(gameObject.name == "Room #16")
            //{
            //    Debug.Log("Trying to put wall down at: " + (roomWidth - 1) + ", " + i);
            //    Debug.Log(m_wallPositions[31].Count);
            //}
            wallPositions[roomWidth - 1][i].PlaceDown(new Vector2(transform.position.x + roomWidth - 1, transform.position.y + i));
        }

        j = 0;
List<RoomEntrance> entrancesToTheNorth = new List<RoomEntrance> { };

        foreach (RoomEntrance entrance in directions.m_directions)
        {
            if (entrance.DirectionModifier == new Vector2(0, 1))
            {
                entrancesToTheNorth.Add(entrance);
            }
        }

        for (int i = 1; i < roomWidth - 1; i++)
        {
            if (i == 9 + j * 20)
            {
                entrancesToTheNorth[j].gameObject.transform.position = new Vector2(transform.position.x + i, transform.position.y + roomHeigth - 1);
                if (entrancesToTheNorth[j].Open == true)
                {
                    if (entrancesToTheNorth[j].Spawned == true)
                    {
                        continue;
                    }
                }
            }
            if (i == 10 + j * 20)
            {
                j++;
                if (entrancesToTheNorth[j - 1].Open == true)
                {
                    if (entrancesToTheNorth[j - 1].Spawned == true)
                    {
                        continue;
                    }
                }
            }
            wallPositions[i][roomHeigth - 1].PlaceDown(new Vector2(transform.position.x + i, transform.position.y + roomHeigth - 1));
        }
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
                switch (temp)
                {
 case "AAAA":
                        wallPositions[i][j].SetVariant(WallVariant.TopCorner);
                        break; //inner wall
                    case "AAAB":
                        wallPositions[i][j].SetVariant(WallVariant.Side);
                        break;
                    case "AABA":
                        wallPositions[i][j].SetVariant(WallVariant.Bottom);
                        break;
                    case "AABB":
                        wallPositions[i][j].SetVariant(WallVariant.BottomLeft);
                        break; 
                    case "ABAA":
                        wallPositions[i][j].SetVariant(WallVariant.Side);
                        break;
                    case "ABAB":
                        wallPositions[i][j].SetVariant(WallVariant.Side);
                        break;
                    case "ABBA":
                        wallPositions[i][j].SetVariant(WallVariant.BottomRight);
                        break; 
                    case "ABBB": break;
                    case "BAAA":
                        wallPositions[i][j].SetVariant(WallVariant.TopCorner); //this is the top room
                        break;
                    case "BAAB":
                        wallPositions[i][j].SetVariant(WallVariant.TopCorner);
                        break;
                    case "BABA":
                        wallPositions[i][j].SetVariant(WallVariant.Bottom);
                        break;
                    case "BABB": break;
                    case "BBAA":
                        wallPositions[i][j].SetVariant(WallVariant.TopCorner);
                        break;
                    case "BBAB": break;
                    case "BBBA": break;
                    case "BBBB": break; //pillar
                }
            }
        }

    }
    public void InstantiateWalls(WallBlueprints blueprints)
    {
        for (int i = 0; i < CameraBoundaries.x; i++)
        {
            for (int j = 0; j < CameraBoundaries.y; j++)
            {
                if (wallPositions[i][j].GetIsOccupied())
                {
                    GameObject newWall = Instantiate(blueprints.wallBlock, new Vector2(transform.position.x, transform.position.y) + wallPositions[i][j].GetPosition(), Quaternion.identity, transform);
                    if (wallPositions[i][j].GetVariant() != WallVariant.None)
                    {
                        //Wall newWall = Instantiate(blueprints.GetWall(m_wallPositions[i][j].GetVariant()), new Vector2(transform.position.x, transform.position.y) + m_wallPositions[i][j].GetPosition(), Quaternion.identity, transform);

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
        for (int i = 0; i < CameraBoundaries.x; i++)
        {
            for (int j = 0; j < CameraBoundaries.y; j++)
            {
                if (!wallPositions[i][j].GetIsOccupied())
                {
                    GameObject newWall = Instantiate(floorTile, new Vector2(transform.position.x, transform.position.y) + wallPositions[i][j].GetPosition(), Quaternion.identity, transform);
                }
            }
        }
    }
}