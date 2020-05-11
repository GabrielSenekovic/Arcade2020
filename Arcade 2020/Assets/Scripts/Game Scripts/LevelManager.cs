using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [System.NonSerialized]public Room firstRoom;
    [System.NonSerialized]public Room lastRoom;

    Room currentRoom;
    [SerializeField] Team team;
    [SerializeField] CameraMovement cameraM;

    [SerializeField] Vector2 RoomSize;

    LevelGenerator generator;
    EntityManager entityManager;

    Vector2Int roomDimensions = new Vector2Int(0,0);

    int currentFloor = 0;

    public UIManager UI;

    void Awake()
    {
        entityManager = GetComponent<EntityManager>();
        generator = GetComponent<LevelGenerator>();
    }

    void Start()
    {
        int seed = Random.Range(0, int.MaxValue);
        Random.InitState(seed);
        Debug.Log("Seed: " + seed);

        ResetLevel();
    }
    void Update()
    {
        if(currentRoom.roomCleared)
        {
            if(Input.GetKeyDown(KeyCode.O))
            {
                if(team.GetIfBothTouchingDoor())
                {
                    if(!team.GetDoor().locked)
                    {
                        cameraM.Move(team.GetDoor().directionModifier, RoomSize);
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
                currentRoom = team.GetDoor().otherDoor.transform.parent.GetComponent<Room>();
                if(!currentRoom.roomCleared)
                {
                    StartCoroutine(entityManager.spawnEnemies(currentRoom));
                }
                UI.StartCoroutine(UI.RevealMap());
                UI.minimap.AddRoomToMap(currentRoom.GetPosition());
                team.MoveTeamToNewRoom();
                entityManager.ToggleFreezeAllEntities(false);
            }
        }
        else
        {
            if(entityManager.amountOfEnemiesSpawned == 0 && entityManager.battleInitiated)
            {
                entityManager.battleInitiated = false;
                currentRoom.roomCleared = true;
                currentRoom.RevealItem();
            }
        }
        if(team.GetIfBothPlayersDead() && UI.deathScreen.alpha == 0)
        {
            Game.SaveHighScore(UI.score.score);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            //UI.OpenOrClose(UI.deathScreen);
        }
    }

    void ResetLevel()
    {
        System.DateTime before = System.DateTime.Now;

        if(currentFloor>0){generator.DestroyLevel();};
        team.ResetTeam();
        cameraM.transform.position = new Vector3(12, 9.5f, cameraM.transform.position.z);
        
        currentFloor++;
        generator.GenerateLevel(this, currentFloor, RoomSize);
        currentRoom = firstRoom;

        UI.floorText.text = "Floor: " + currentFloor;

        System.DateTime after = System.DateTime.Now; 
        System.TimeSpan duration = after.Subtract(before);
        Debug.Log("Time to reset: " + duration.TotalMilliseconds + " milliseconds, which is: " + duration.TotalSeconds + " seconds");
    }
}