using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Room firstRoom;
    public Room lastRoom;
    [SerializeField] Team team;
    [SerializeField] CameraMovement cameraM;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O) && team.GetIfBothTouchingDoor())
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
        if(cameraM.movementDone)
        {
            cameraM.movementDone = false;
            team.MoveTeamToNewRoom();
        }
    }
}
