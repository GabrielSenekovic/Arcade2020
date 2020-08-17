using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBall : Ball
{
    float speedModifier = 0;
    float accelerationOfSpeed = 0.01f;
    // Start is called before the first frame update
    void Update()
    {
        if (isTraveling && isOn == OwnedByPlayer.PLAYER_ONE)
        {
            Speed = flySpeed * speedModifier;
            Dir = (players[1].transform.position - transform.position).normalized;
        }
        else if (isTraveling && isOn == OwnedByPlayer.PLAYER_TWO)
        {
            Speed = flySpeed * speedModifier;
            Dir = (players[0].transform.position - transform.position).normalized;
        }
        else
        {
            Speed = 0.0f;
        }
        MoveObject();
    }
    void FixedUpdate()
    {
        if (isTraveling && isOn == OwnedByPlayer.PLAYER_ONE)
        {
            speedModifier += accelerationOfSpeed;
        }
        else if (isTraveling && isOn == OwnedByPlayer.PLAYER_TWO)
        {
            speedModifier += accelerationOfSpeed;
        }
        else
        {
            speedModifier = 0;
        }
    }
}
