using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public List<Movement> entities;
    [SerializeField]Score score;

    public int amountOfEnemies = 0;

    public Team team;
/*
    public void Update()
    {
        foreach(Movement entity in entities)
        {
            //if entity is dead
            if(false)
            {
                score.GetScoreFromEnemy(entity.type);
            }
        }
    }
*/
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
                Destroy(entities[i]);
                //! score
            }
        }
    }
}
