using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : PickUp
{
    public override void OnPickUp(GameObject otherObject) 
    {
        otherObject.gameObject.GetComponentInParent<Team>().amountOfKeys++;
        Destroy(gameObject);
    }
}
