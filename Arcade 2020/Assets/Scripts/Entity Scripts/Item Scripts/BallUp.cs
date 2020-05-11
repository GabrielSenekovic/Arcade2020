using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallUp : PickUp
{
    public override void OnPickUp(GameObject otherObject) 
    {
        otherObject.transform.parent.transform.gameObject.GetComponent<Team>().IncreaseBallAmount(otherObject);
        Destroy(gameObject);
    }
}
