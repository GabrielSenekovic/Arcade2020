using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUpgrade : PickUp
{
    public enum ShieldUpgradeType
    {
        Nevermint = 0,
        Reflect = 1
    }
    public ShieldUpgradeType myType;
    public override void OnPickUp(GameObject player)
    {
        player.transform.parent.transform.gameObject.GetComponent<Team>().PowerUpShield(player, myType, this);
    }
}