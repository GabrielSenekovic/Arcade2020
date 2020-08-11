using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

public partial class LevelManager : MonoBehaviour
{
    [System.NonSerialized]public Room firstRoom;
    [System.NonSerialized]public Room lastRoom;

    public float enemyLoadTime;

    Manuscript script = new Manuscript();

    Room currentRoom;
    [SerializeField] Team team;
    [SerializeField] CameraMovement cameraM;

    [System.NonSerialized]public Vector2 roomSize = new Vector2(28, 20);

    bool tutorial = false;

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

        if(DebugManager.PlayTutorial)
        {
            gameObject.AddComponent(typeof(Tutorial));
            GetComponent<Tutorial>().InitiateTutorial(entityManager, UI, team, currentRoom, roomSize);
        }
    }
    void Update()
    {
        bool isBothTouchingDoor = team.GetIfBothTouchingDoor();
        if(GetComponent<Tutorial>())
        {
            if(!GetComponent<Tutorial>().OnPlay(entityManager, UI, script.tutorialDialog, team))
            {
                Destroy(GetComponent<Tutorial>());
            }
        }

        if(currentRoom.roomCleared)
        {
            if(isBothTouchingDoor && !cameraM.moving && !cameraM.movementDone)
            {
                if(!team.GetDoor().locked && team.canEnterDoor)
                {
                    entityManager.ToggleFreezeAllEntities(true);
                    cameraM.Move(team.GetDoor().directionModifier, roomSize);
                }
                else
                {
                    if(team.amountOfKeys > 0)
                    {
                        Debug.Log("Depleting amount of keys");
                        Door door = team.GetDoor();
                        team.amountOfKeys--;
                        UI.keyAmount.text = ": " + team.amountOfKeys;
                        door.Unlock();
                        door.otherDoor.Unlock();
                        StartCoroutine(team.WaitUntilCanEnterDoor());
                    }
                }
            }
            if(team.GetIfTouchingStairs())
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
        }
    }

    void ResetLevel()
    {
       // GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
        //System.GC.Collect();
        System.DateTime before = System.DateTime.Now;

        if(currentFloor>0){generator.DestroyLevel();};
        UI.minimap.ResetMap();
        team.ResetTeam(roomSize);
        cameraM.transform.position = new Vector3(roomSize.x/2, 9.5f, cameraM.transform.position.z);
        
        currentFloor++;
        generator.GenerateLevel(this, currentFloor, roomSize);
        currentRoom = firstRoom;
        UI.RevealMap(enemyLoadTime, true);

        UI.floorText.text = "Floor: " + currentFloor;

        System.DateTime after = System.DateTime.Now; 
        System.TimeSpan duration = after.Subtract(before);
        Debug.Log("Time to reset: " + duration.TotalMilliseconds + " milliseconds, which is: " + duration.TotalSeconds + " seconds");
       // GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
    }
}

partial class LevelManager
{
    public void OnMoveToNewRoom()
    {
        cameraM.movementDone = false;
        UI.minimap.currentRoom.GetComponent<SpriteRenderer>().color = UI.colors[0];
        currentRoom = team.GetDoor().otherDoor.transform.parent.GetComponent<Room>();
        if(!currentRoom.roomCleared)
        {
            StartCoroutine(entityManager.spawnEnemies(currentRoom, enemyLoadTime, team, roomSize));
            foreach(GameObject door in currentRoom.doors)
            {
                door.GetComponent<Door>().OpenClose(false);
            }
        }
        UI.minimap.AddRoomToMap(currentRoom.GetPosition(), currentRoom.doors);
        UI.StartCoroutine(UI.RevealMap(enemyLoadTime, currentRoom.roomCleared));
        team.MoveTeamToNewRoom();
        StartCoroutine(team.WaitUntilCanEnterDoor());
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
            if(!team.GetIfHasTakenDamage() && entityManager.roomDifficultyLevel > 4 && !currentRoom.myItem.GetComponent<Key>())
            {
                Destroy(currentRoom.myItem);
                currentRoom.myItem = null;
                currentRoom.myItem = Instantiate(GetComponent<RoomBuilder>().blueprint.ballUp, new Vector3(currentRoom.transform.position.x + roomSize.x / 2, currentRoom.transform.position.y + roomSize.y / 2, currentRoom.transform.position.z), Quaternion.identity, currentRoom.transform);
            }
            else
            {
                currentRoom.RevealItem();
            }
            foreach(GameObject door in currentRoom.doors)
            {
                door.GetComponent<Door>().OpenClose(true);
            }
            entityManager.roomDifficultyLevel = 0;
        }
    } 

}