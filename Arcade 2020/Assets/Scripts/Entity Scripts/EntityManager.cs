using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public List<GameObject> TypesOfEnemies;
    public List<Movement> entities;

    public bool battleInitiated = false;

    public int amountOfEnemiesSpawned = 0;

    public Team team;

    public void ToggleFreezeAllEntities(bool value)
    {
        foreach(Movement entity in entities)
        {
            entity.ToggleFrozen(value);
        }
    }

    private void Update() {
        for (int i = 0; i < entities.Count; i++)
        {
            if(entities[i].GetComponent<EnemyHealthController>())
            {
                if(entities[i].GetComponent<EnemyHealthController>().isdead)
                {
                    GetComponent<LevelManager>().UI.score.GetScoreFromEnemy(entities[i].GetComponent<EnemyHealthController>().type);
                    GameObject temp = entities[i].gameObject;
                    entities.Remove(entities[i]);
                    Destroy(temp);
                    amountOfEnemiesSpawned--;
                    //! score
                }
            }
        }
    }
    public IEnumerator spawnEnemies(Room newRoom, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        int amountOfEnemiesToSpawn = Random.Range(1,4);
        List<Vector2> spawnLocations = new List<Vector2>(){};
        for(int j = 0; j <= amountOfEnemiesToSpawn; j++)
        {
            Vector2 newSpawnLocation = Vector2.zero;
            while(!spawnLocations.Contains(newSpawnLocation) && newSpawnLocation == Vector2.zero)
            {
                newSpawnLocation = new Vector2(newRoom.transform.position.x + Random.Range(4,20), newRoom.transform.position.y + Random.Range(4,17));
            }
            spawnLocations.Add(newSpawnLocation);

            GameObject newEnemy = Instantiate(TypesOfEnemies[Random.Range(0, TypesOfEnemies.Count)], newSpawnLocation, Quaternion.identity, newRoom.transform);
            entities.Add(newEnemy.GetComponent<Movement>());
            amountOfEnemiesSpawned++;
            if(newEnemy.GetComponent<SpnogController>())
            {
                newEnemy.GetComponent<SpnogController>().players[0] = team.players[0].gameObject;
                newEnemy.GetComponent<SpnogController>().players[1] = team.players[1].gameObject;
            }
            if(newEnemy.GetComponent<ScorpacaController>())
            {
                newEnemy.GetComponent<ScorpacaController>().players[0] = team.players[0].gameObject;
                newEnemy.GetComponent<ScorpacaController>().players[1] = team.players[1].gameObject;
            }
            newEnemy.GetComponent<EnemyController>().Spawn();
            FindObjectOfType<AudioManager>().Play("Falling");
        }
        battleInitiated = true;
    }
}
