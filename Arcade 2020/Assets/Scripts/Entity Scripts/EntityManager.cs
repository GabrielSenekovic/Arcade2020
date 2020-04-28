using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField]List<Movement> entities;
    [SerializeField]Score score;

    public Team team;

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

    public void ToggleFreezeAllEntities(bool value)
    {
        foreach(Movement entity in entities)
        {
            entity.ToggleFrozen(value);
        }
    }
}
