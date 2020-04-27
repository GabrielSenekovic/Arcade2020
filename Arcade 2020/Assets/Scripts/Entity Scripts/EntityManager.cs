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
}
