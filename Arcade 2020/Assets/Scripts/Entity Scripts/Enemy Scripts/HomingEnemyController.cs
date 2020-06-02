using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemyController : EnemyController
{
    [System.NonSerialized]public Player[] players;

    public virtual void Initialise(Player[] givenPlayers)
    {
        players = givenPlayers;
    }
}
