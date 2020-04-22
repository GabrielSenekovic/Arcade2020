using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : PickUp
{
    public override void OnPickUp(GameObject otherObject) 
    {
        if(otherObject.gameObject.GetComponent<PlayerMovementController>())
        {
            otherObject.gameObject.GetComponentInParent<Team>().amountOfKeys++;
        }
    }
}
