using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField]List<Movement> entities;
    [SerializeField]Score score;

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
                score.GetScoreFromEnemy(entities[i].GetComponent<HealthManager>().type);

                GameObject temp = entities[i].gameObject;
                entities.Remove(entities[i]);
                Destroy(temp);
                //! score
            }
        }
    }
}
