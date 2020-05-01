using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : HealthManager
{

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnDeath()
    {
          isdead = true;
    }
}
