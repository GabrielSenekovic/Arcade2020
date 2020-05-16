using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : PickUp
{
    [SerializeField]float speedIncrease;
    public override void OnPickUp(GameObject otherObject) 
    {
        otherObject.GetComponent<PlayerMovementController>().playerspeed += speedIncrease;
        Destroy(gameObject);
    }
}
