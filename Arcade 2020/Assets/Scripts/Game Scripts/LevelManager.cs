using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class LevelManager : MonoBehaviour
{
    [System.NonSerialized]public Room firstRoom;
    [System.NonSerialized]public Room lastRoom;

    public float enemyLoadTime;

    Manuscript script = new Manuscript();

    Room currentRoom;
    [SerializeField] Team team;
    [SerializeField] CameraMovement cameraM;

    public Vector2 RoomSize;

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

        PlayTutorial();
    }
    void Update()
    {
        bool isBothTouchingDoor = team.GetIfBothTouchingDoor();

        if(currentRoom.roomCleared)
        {
            if(isBothTouchingDoor && !cameraM.moving && !cameraM.movementDone)
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
            if(cameraM.movementDone)
            {
                OnMoveToNewRoom();
            }
        }
        else
        {
            OnEnemiesDefeated();
        }
        if(team.GetIfBothPlayersDead() && UI.deathScreen.alpha == 0)
        {
            UI.OpenOrClose(UI.deathScreen);
            Game.SaveHighScore(UI.score.score);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    void ResetLevel()
    {
        System.DateTime before = System.DateTime.Now;

        if(currentFloor>0){generator.DestroyLevel();};
        UI.minimap.ResetMap();
        team.ResetTeam();
        cameraM.transform.position = new Vector3(RoomSize.x/2, 9.5f, cameraM.transform.position.z);
        
        currentFloor++;
        generator.GenerateLevel(this, currentFloor, RoomSize);
        currentRoom = firstRoom;
        UI.RevealMap(enemyLoadTime, true);

        UI.floorText.text = "Floor: " + currentFloor;

        System.DateTime after = System.DateTime.Now; 
        System.TimeSpan duration = after.Subtract(before);
        Debug.Log("Time to reset: " + duration.TotalMilliseconds + " milliseconds, which is: " + duration.TotalSeconds + " seconds");
    }
}

partial class LevelManager
{
    public void PlayTutorial()
    {
        GameObject newEnemy = Instantiate(entityManager.TypesOfEnemies[0], new Vector2(RoomSize.x/2, RoomSize.y/2), Quaternion.identity, currentRoom.transform);
        entityManager.entities.Add(newEnemy.GetComponent<Movement>());
        entityManager.amountOfEnemiesSpawned++;
        UI.minimap.gameObject.SetActive(false);
        currentRoom.roomCleared = false;
        foreach(GameObject door in currentRoom.doors)
        {
            door.GetComponent<Door>().OpenClose(false);
        }
        entityManager.battleInitiated = true;
        UI.OpenOrClose(UI.speechBubble);
        StartCoroutine(UI.speechBubble_Obj.PrintMessage(script.dialogs[0]));
    }
    public void OnMoveToNewRoom()
    {
        cameraM.movementDone = false;
        UI.minimap.currentRoom.GetComponent<SpriteRenderer>().color = UI.colors[0];
        currentRoom = team.GetDoor().otherDoor.transform.parent.GetComponent<Room>();
        if(!currentRoom.roomCleared)
        {
            StartCoroutine(entityManager.spawnEnemies(currentRoom, enemyLoadTime));
            foreach(GameObject door in currentRoom.doors)
            {
                door.GetComponent<Door>().OpenClose(false);
            }
        }
        UI.minimap.AddRoomToMap(currentRoom.GetPosition());
        UI.StartCoroutine(UI.RevealMap(enemyLoadTime, currentRoom.roomCleared));
        team.MoveTeamToNewRoom();
        Debug.Log("Moving team");
        entityManager.ToggleFreezeAllEntities(false);
    }
    public void OnEnemiesDefeated()
    {
        if(entityManager.amountOfEnemiesSpawned == 0 && entityManager.battleInitiated)
        {
            entityManager.battleInitiated = false;
            currentRoom.roomCleared = true;
            UI.minimap.gameObject.SetActive(true);
            UI.minimap.currentRoom.GetComponent<SpriteRenderer>().color = UI.colors[1];
            currentRoom.RevealItem();
            foreach(GameObject door in currentRoom.doors)
            {
                door.GetComponent<Door>().OpenClose(true);
            }
        }
    } 

}