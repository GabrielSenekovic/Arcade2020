using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : PickUp
{
    public override void OnPickUp(GameObject otherObject) 
    {
        otherObject.GetComponent<PlayerMovementController>().playerspeed += 0.5f;
        Destroy(gameObject);
    }
}
