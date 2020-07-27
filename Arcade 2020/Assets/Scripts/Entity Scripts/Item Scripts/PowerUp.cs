using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : PickUp
{
    public enum PowerUpType
    {
        Basic = 0,
        Lightning = 1,
        Orbital = 2,
        BlackHole = 3,
        Heavy = 4,
        Train = 5
    }
    public PowerUpType myType;

    public override void OnPickUp(GameObject player)
    {
        player.transform.parent.transform.gameObject.GetComponent<Team>().PowerUpBall(player, myType, this);
    }
}
