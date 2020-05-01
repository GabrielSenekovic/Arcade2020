using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField]List<Movement> entities;

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
