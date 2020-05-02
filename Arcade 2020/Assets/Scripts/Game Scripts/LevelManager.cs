using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public Room firstRoom;
    public Room lastRoom;

    Room currentRoom;
    [SerializeField] Team team;
    [SerializeField] CameraMovement cameraM;

    [SerializeField] LevelGenerator generator;
    EntityManager entityManager;

    Vector2Int roomDimensions = new Vector2Int(0,0);

    int currentFloor = 0;

    [SerializeField] Text floorText;

    bool battleInitiated = false;

    void Awake()
    {
        entityManager = GetComponent<EntityManager>();
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
                currentRoom = team.GetDoor().otherDoor.transform.parent.GetComponent<Room>();
                if(!currentRoom.roomCleared)
                {
                StartCoroutine(spawnEnemies(currentRoom));
                }
                team.MoveTeamToNewRoom();
                entityManager.ToggleFreezeAllEntities(false);
            }
        }
        else
        {
            if(entityManager.amountOfEnemies == 0 && battleInitiated)
            {
                battleInitiated = false;
                currentRoom.roomCleared = true;
                currentRoom.RevealItem();
            }
        }
    }

    void ResetLevel()
    {
        System.DateTime before = System.DateTime.Now;

        if(currentFloor>0){generator.DestroyLevel();};
        team.ResetTeam();
        cameraM.transform.position = new Vector3(10, 9.5f, cameraM.transform.position.z);
        
        currentFloor++;
        generator.GenerateLevel(this, currentFloor);
        currentRoom = firstRoom;

        floorText.text = "Floor: " + currentFloor;

        System.DateTime after = System.DateTime.Now; 
        System.TimeSpan duration = after.Subtract(before);
        Debug.Log("Time to reset: " + duration.TotalMilliseconds + " milliseconds, which is: " + duration.TotalSeconds + " seconds");
    }
    IEnumerator spawnEnemies(Room newRoom)
    {
        yield return new WaitForSeconds(2);
        int amountOfEnemies = Random.Range(1,4);
        for(int j = 0; j <= amountOfEnemies; j++)
        {
            GameObject newEnemy = Instantiate(enemies[Random.Range(0, enemies.Count)], 
            new Vector2(newRoom.transform.position.x + Random.Range(4,17), newRoom.transform.position.y + Random.Range(4,17)),
            Quaternion.identity, newRoom.transform);
            entityManager.entities.Add(newEnemy.GetComponent<Movement>());
            entityManager.amountOfEnemies++;
            if(newEnemy.GetComponent<SpnogController>())
            {
                newEnemy.GetComponent<SpnogController>().players[0] = team.players[0].gameObject;
                newEnemy.GetComponent<SpnogController>().players[1] = team.players[1].gameObject;
            }
        }
        battleInitiated = true;
    }
}