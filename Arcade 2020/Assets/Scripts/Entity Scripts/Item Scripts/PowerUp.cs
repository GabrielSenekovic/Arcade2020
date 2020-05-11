using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
    {
        Basic = 0,
        Lightning = 1,
        Orbital = 2
    }
public class PowerUp : PickUp
{
    [SerializeField] PowerUpType myType;

    public override void OnPickUp(GameObject player)
    {
        if(player.transform.parent.transform.gameObject.GetComponent<Team>().PowerUpBall(player, myType))
        {
            Destroy(gameObject);
        }
    }
}
