using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Room firstRoom;
    public Room lastRoom;
    [SerializeField] Team team;
    [SerializeField] CameraMovement cameraM;

    [SerializeField] LevelGenerator generator;

    public int currentFloor = 1;

    [SerializeField] Text floorText;

    void Awake()
    {
        firstRoom = generator.mainRoom;
        generator.Initiate(firstRoom, this);
        floorText.text = "Floor: 1";
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            if(team.GetIfBothTouchingDoor())
            {
                if(!team.GetDoor().locked)
                {
                    cameraM.Move(team.GetDoor().directionModifier);
                }
                else
                {
                    if(team.amountOfKeys > 0)
                    {
                        team.amountOfKeys--;
                        team.GetDoor().Unlock();
                    }
                }
            }
            if(team.GetIfBothTouchingStairs())
            {
                team.players[0].touchingStairs = false;
                team.players[1].touchingStairs = false;
                generator.DestroyLevel();
                currentFloor++;
                firstRoom = Instantiate(generator.RoomPrefab, Vector3.zero, Quaternion.identity, generator.transform);
                team.players[0].transform.position = new Vector2(firstRoom.transform.position.x + 10, firstRoom.transform.position.y + 10);
                team.players[1].transform.position = new Vector2(firstRoom.transform.position.x + 10, firstRoom.transform.position.y + 10);
                cameraM.transform.position = new Vector3(firstRoom.transform.position.x + 10, firstRoom.transform.position.y + 9.5f, cameraM.transform.position.z);
                generator.rooms.Add(firstRoom);
                generator.Initiate(firstRoom, this);
                floorText.text = "Floor: " + currentFloor;
            }
        }
        if(cameraM.movementDone)
        {
            cameraM.movementDone = false;
            team.MoveTeamToNewRoom();
        }
    }
}