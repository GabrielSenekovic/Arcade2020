using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRepository : MonoBehaviour
{
    public List<Ball> balls = new List<Ball>(){};
    public List<PowerUp> powerUps = new List<PowerUp>(){};

    public Ball GetBall(PowerUp.PowerUpType type)
    {
        for(int i = 0; i < balls.Count; i++)
        {
            if(balls[i].myType == type)
            {
                Ball temp = balls[i];
                balls.RemoveAt(i);
                return temp;
            }
        }
        return null;
    }
    public PowerUp GetPowerUp(PowerUp.PowerUpType type)
    {
        for(int i = 0; i < powerUps.Count; i++)
        {
            if(powerUps[i].myType == type)
            {
                PowerUp temp = powerUps[i];
                powerUps.RemoveAt(i);
                return temp;
            }
        }
        return null;
    }
}
