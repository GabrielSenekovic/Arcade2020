using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : PickUp
{
    [SerializeField]float speedIncrease;
    public override void OnPickUp(GameObject otherObject) 
    {
        if(otherObject.GetComponent<Player>().boosts.AddBoost(BoostsDisplay.BoostType.SpeedBoost))
        {
            otherObject.GetComponent<PlayerMovementController>().playerSpeed += speedIncrease;
            Destroy(gameObject);
        }
    }
}
