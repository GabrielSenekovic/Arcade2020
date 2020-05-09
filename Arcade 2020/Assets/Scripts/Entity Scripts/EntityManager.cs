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
            if(entities[i].GetComponent<HealthManager>().isdead)
            {
                GetComponent<LevelManager>().UI.score.GetScoreFromEnemy(entities[i].GetComponent<HealthManager>().type);

                GameObject temp = entities[i].gameObject;
                entities.Remove(entities[i]);
                Destroy(temp);
                amountOfEnemiesSpawned--;
                //! score
            }
        }
    }
    public IEnumerator spawnEnemies(Room newRoom)
    {
        yield return new WaitForSecondsRealtime(2);
        Debug.Log("Spawn enemies now executing");
        int amountOfEnemiesToSpawn = Random.Range(1,4);
        for(int j = 0; j <= amountOfEnemiesToSpawn; j++)
        {
            GameObject newEnemy = Instantiate(TypesOfEnemies[Random.Range(0, TypesOfEnemies.Count)], 
            new Vector2(newRoom.transform.position.x + Random.Range(4,17), newRoom.transform.position.y + Random.Range(4,17)),
            Quaternion.identity, newRoom.transform);
            entities.Add(newEnemy.GetComponent<Movement>());
            amountOfEnemiesSpawned++;
            if(newEnemy.GetComponent<SpnogController>())
            {
                newEnemy.GetComponent<SpnogController>().players[0] = team.players[0].gameObject;
                newEnemy.GetComponent<SpnogController>().players[1] = team.players[1].gameObject;
            }
        }
        battleInitiated = true;
    }
}
