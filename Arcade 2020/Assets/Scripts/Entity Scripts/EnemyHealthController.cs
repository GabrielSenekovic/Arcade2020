using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : HealthManager
{
    [System.NonSerialized]public EnemyType type;
    // Update is called once per frame

    public override void OnDeath()
    {
        isdead = true;

    }

    private void BamPow()
    {
        
    }

    public override void ChildUpdate()
    {
        //! do stuff;
    }
}
