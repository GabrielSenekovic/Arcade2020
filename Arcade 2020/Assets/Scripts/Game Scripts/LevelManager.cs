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
    EntityManager entityManager;

    int currentFloor = 0;

    [SerializeField] Text floorText;

    void Awake()
    {
        entityManager = GetComponent<EntityManager>();
    }

    void Start()
    {
        ResetLevel();
        Debug.Log(Time.realtimeSinceStartup);
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
                    entityManager.ToggleFreezeAllEntities(true);
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
                ResetLevel();
            }
        }
        if(cameraM.movementDone)
        {
            cameraM.movementDone = false;
            team.MoveTeamToNewRoom();
            entityManager.ToggleFreezeAllEntities(false);
        }
    }

    void ResetLevel()
    {
        if(currentFloor>0){generator.DestroyLevel();};
        team.ResetTeam();
        cameraM.transform.position = new Vector3(10, 9.5f, cameraM.transform.position.z);
        
        currentFloor++;
        generator.GenerateLevel(this, currentFloor);

        floorText.text = "Floor: " + currentFloor;
    }
}