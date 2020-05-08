using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : HealthManager
{

    // Update is called once per frame

    public override void OnDeath()
    {
        isdead = true;
    }

    public override void ChildUpdate()
    {
        //! do stuff;
    }
}
