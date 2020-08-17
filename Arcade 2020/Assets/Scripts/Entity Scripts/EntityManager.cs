using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public List<GameObject> TypesOfEnemies;
    public List<GameObject> TypesOfBosses;

    public List<Movement> currentlySpawnedEntities;

    public bool battleInitiated = false;

    public int amountOfEnemiesSpawned = 0;

    public int roomDifficultyLevel = 0;
    public void ToggleFreezeAllEntities(bool value)
    {
        foreach(Movement entity in currentlySpawnedEntities)
        {
            entity.ToggleFrozen(value);
        }
    }

    private void Update() 
    {
        for (int i = 0; i < currentlySpawnedEntities.Count; i++)
        {
            if(currentlySpawnedEntities[i].GetComponent<EnemyHealthController>())
            {
                if(currentlySpawnedEntities[i].GetComponent<EnemyHealthController>().isdead)
                {
                    GetComponent<LevelManager>().UI.score.GetScoreFromEnemy(currentlySpawnedEntities[i].GetComponent<EnemyHealthController>().type);
                    GameObject temp = currentlySpawnedEntities[i].gameObject;
                    currentlySpawnedEntities.Remove(currentlySpawnedEntities[i]);
                    Destroy(temp);
                    amountOfEnemiesSpawned--;
                    //! score
                }
            }
        }
    }
    public IEnumerator spawnEnemies(Room newRoom, float time, Team team, Vector2 RoomSize, bool isBoss)
    {
        yield return new WaitForSecondsRealtime(time);
        if(!isBoss){OnSpawnEnemies(newRoom, team, RoomSize);}
        else { OnSpawnBoss(newRoom, team, RoomSize);}
        battleInitiated = true;
    }
    public void OnSpawnEnemies(Room newRoom, Team team, Vector2 RoomSize)
    {
        int amountOfEnemiesToSpawn = Random.Range(1, 4);
        List<Vector2> spawnLocations = new List<Vector2>() { };
        for (int j = 0; j <= amountOfEnemiesToSpawn; j++)
        {
            Vector2 newSpawnLocation = Vector2.zero;
            while (!spawnLocations.Contains(newSpawnLocation) && newSpawnLocation == Vector2.zero)
            {
                newSpawnLocation = new Vector2(newRoom.transform.position.x + Random.Range(6, RoomSize.x - 6), newRoom.transform.position.y + Random.Range(6, RoomSize.y - 6));
            }
            spawnLocations.Add(newSpawnLocation);

            GameObject newEnemy = Instantiate(TypesOfEnemies[Random.Range(0, TypesOfEnemies.Count)], newSpawnLocation, Quaternion.identity, newRoom.transform);
            currentlySpawnedEntities.Add(newEnemy.GetComponent<Movement>());
            roomDifficultyLevel += newEnemy.GetComponent<EnemyController>().difficultyLevel;
            amountOfEnemiesSpawned++;

            if (newEnemy.GetComponent<HomingEnemyController>())
            {
                newEnemy.GetComponent<HomingEnemyController>().Initialise(team.players);
            }
            newEnemy.GetComponent<EnemyController>().Spawn();
            FindObjectOfType<AudioManager>().Play("Falling");
        }
    }
    public void OnSpawnBoss(Room newRoom, Team team, Vector2 RoomSize)
    {
        FindObjectOfType<AudioManager>().PlayMusic("Boss");
        Vector2 newSpawnLocation = new Vector2(newRoom.transform.position.x + RoomSize.x/2, newRoom.transform.position.y + RoomSize.y/2);
        GameObject Boss = Instantiate(TypesOfBosses[Random.Range(0, TypesOfBosses.Count)], newSpawnLocation, Quaternion.identity, newRoom.transform);
        currentlySpawnedEntities.Add(Boss.GetComponent<Movement>());
        roomDifficultyLevel += Boss.GetComponent<EnemyController>().difficultyLevel;
        amountOfEnemiesSpawned++;
        if (Boss.GetComponent<HomingEnemyController>())
        {
            Boss.GetComponent<HomingEnemyController>().Initialise(team.players);
        }
        Boss.GetComponent<EnemyController>().Spawn();
        FindObjectOfType<AudioManager>().Play("Falling");
    }
}
